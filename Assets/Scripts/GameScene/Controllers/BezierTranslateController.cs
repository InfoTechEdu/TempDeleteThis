using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

[Serializable]
public class BezierTranslatePoint
{
    public float valueOnBezier;
    [ReadOnly] public Transform busyTransform;

    public BezierTranslatePoint(float valueOnBezier, Transform busyTransform)
    {
        this.valueOnBezier = valueOnBezier;
        this.busyTransform = busyTransform;
    }
}

public class BezierTranslateController : Controller
{
    public event Action<bool> OnChangeTranslateStatus;
    public event Action<Transform> OnTransformFinishedBezier;
    public event Action<Transform> OnChangeTransformOnFocusPoint;

    [SerializeField] private BezierCurve bezierCurve;
    [Space]
    [Header("Translate properties")]
    [SerializeField] private float translateSpeed;
    [SerializeField] private float focusScaleMultiplier;
    [SerializeField] private float focusScaleDuration;
    [SerializeField] private Ease focusScaleEase;
    [Space]
    [SerializeField] private BezierTranslatePoint startPoint;
    [SerializeField] private BezierTranslatePoint unfocusPoint;
    [SerializeField] private BezierTranslatePoint focusPoint;
    [SerializeField] private BezierTranslatePoint endPoint;
    [Space]
    [SerializeField] [ReadOnly] private bool isTranslated;

    public bool CanTranslate
    {
        get
        {
            return !isTranslated;
        }
    }

    public void SetTransformsToPoints(Transform start, Transform unfocus, Transform focus)
    {
        start.position = bezierCurve.GetPointAt(startPoint.valueOnBezier);
        unfocus.position = bezierCurve.GetPointAt(startPoint.valueOnBezier);
        focus.position = bezierCurve.GetPointAt(startPoint.valueOnBezier);

        StartCoroutine(TranslateTransformAlongCurve(focus, startPoint, focusPoint, 0, (transform, point) => { TransformChangePoint(transform, point); ChangeTranslateStatus(false); }));
        StartCoroutine(TranslateTransformAlongCurve(unfocus, startPoint, unfocusPoint, 0.5f, (transform, point) => TransformChangePoint(transform, point)));
        StartCoroutine(TranslateTransformAlongCurve(start, startPoint, startPoint, 0.5f, (transform, point) => TransformChangePoint(transform, point)));

        focus.localScale *= focusScaleMultiplier;

        ChangeTranslateStatus(true);
    }

    public void AddTransformInSequence(Transform transform)
    {
        if (isTranslated)
            return;

        if (endPoint.busyTransform)
            Destroy(endPoint.busyTransform.gameObject);

        StartCoroutine(TranslateTransformAlongCurve(focusPoint.busyTransform, focusPoint, endPoint, 0, (transform, point) => TransformChangePoint(transform, point)));
        StartCoroutine(TranslateTransformAlongCurve(unfocusPoint.busyTransform, unfocusPoint, focusPoint, 0, (transform, point) => { TransformChangePoint(transform, point); ChangeTranslateStatus(false); }));
        StartCoroutine(TranslateTransformAlongCurve(startPoint.busyTransform, startPoint, unfocusPoint, 0, (transform, point) => TransformChangePoint(transform, point)));
        StartCoroutine(TranslateTransformAlongCurve(transform, startPoint, startPoint, 0, (transform, point) => TransformChangePoint(transform, point)));

        Transform unfocusPointTransform = unfocusPoint.busyTransform;

        if (unfocusPointTransform)
        {   
            unfocusPointTransform.DOScale(unfocusPointTransform.localScale * focusScaleMultiplier, focusScaleDuration).SetEase(focusScaleEase);
        }

        ChangeTranslateStatus(true);
    }

    public void HideAllTransforms()
    {
        StartCoroutine(TranslateTransformAlongCurve(focusPoint.busyTransform, focusPoint, endPoint, 0, (transform, point) => TransformChangePoint(transform, point)));
        StartCoroutine(TranslateTransformAlongCurve(unfocusPoint.busyTransform, unfocusPoint, endPoint, 0, (transform, point) => TransformChangePoint(transform, point)));
        StartCoroutine(TranslateTransformAlongCurve(startPoint.busyTransform, startPoint, endPoint, 0.25f, (transform, point) => { TransformChangePoint(transform, point); ChangeTranslateStatus(false); }));

        startPoint.busyTransform.DOScale(Vector3.zero, 0.1f);

        ChangeTranslateStatus(true);
    }


    private void ChangeTranslateStatus(bool flag)
    {
        isTranslated = flag;

        OnChangeTranslateStatus?.Invoke(isTranslated);
    }

    private void TransformChangePoint(Transform transform, BezierTranslatePoint endPoint)
    {
        if (endPoint.Equals(this.endPoint))
        {
            TransformFinishedBezier(transform);
        }
        else if (endPoint.Equals(focusPoint))
        {
            OnChangeTransformOnFocusPoint?.Invoke(transform);
        }
    }

    private void TransformFinishedBezier(Transform transform)
    {
        OnTransformFinishedBezier?.Invoke(transform);
    }


    protected override void OnInitialize() { }

    protected override void OnActivate() { }

    protected override void OnDiactivate() { }


    private IEnumerator TranslateTransformAlongCurve(Transform transform, BezierTranslatePoint startPoint, BezierTranslatePoint endPoint, float timer, Action<Transform, BezierTranslatePoint> onComplete)
    {
        if (!transform)
            yield break;

        yield return new WaitForSeconds(timer);

        if (startPoint.valueOnBezier == endPoint.valueOnBezier)
        {
            transform.position = bezierCurve.GetPointAt(endPoint.valueOnBezier);
        }

        startPoint.busyTransform = null;

        float t = startPoint.valueOnBezier;

        if (endPoint.valueOnBezier > 0.99f)
            endPoint.valueOnBezier = 0.99f;

        while (t < endPoint.valueOnBezier)
        {
            t += Time.deltaTime / (10 / translateSpeed);

            transform.position = bezierCurve.GetPointAt(t);

            yield return new WaitForEndOfFrame();
        }

        endPoint.busyTransform = transform;

        if (onComplete != null)
            onComplete?.Invoke(transform, endPoint);
    }      
}
    