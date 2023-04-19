using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    [SerializeField] private HealthSystem healthSystem;

    private Transform barTransform;
    private Transform separatorContainer;
    private void Awake() {
        barTransform = transform.Find("bar");
    }

    private void Start() {
        separatorContainer = transform.Find("separatorContainer");

        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        healthSystem.OnHealthAmounMaxChanged += HealthSystem_OnHealthAmounMaxChanged;

        ConstructHealthBarSeparators();
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void HealthSystem_OnHealthAmounMaxChanged(object sender, EventArgs e) {
        ConstructHealthBarSeparators();
    }

    private void ConstructHealthBarSeparators() {
        Transform separatorTemplate = separatorContainer.Find("separatorTemplate");
        separatorTemplate.gameObject.SetActive(false);

        foreach(Transform separatorTransform in separatorContainer) {
            if (separatorTransform == separatorTemplate)
                continue;

            Destroy(separatorTransform.gameObject);
        }

        int healthAmounPerSeparator = 10;
        float barSize = 2f;
        float barOnHealthAmountSize = barSize / healthSystem.GetHealthAmounMax();
        int healthSeparatorCount = Mathf.FloorToInt(healthSystem.GetHealthAmounMax() / healthAmounPerSeparator);

        for (int i = 1; i < healthSeparatorCount; i++) {
            Transform separatorTransform = Instantiate(separatorTemplate, separatorContainer);
            separatorTransform.gameObject.SetActive(true);
            separatorTransform.localPosition = new Vector3(barOnHealthAmountSize * i * healthAmounPerSeparator, 0, 0);
        }
    }

    private void HealthSystem_OnHealed(object sender, EventArgs e) {
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e) {
        UpdateBar();
        UpdateHealthBarVisible();
    }
    private void UpdateBar() {
        barTransform.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(), 1, 1);
    }

    private void UpdateHealthBarVisible() {
        if (healthSystem.IsFullHealth())
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
}
