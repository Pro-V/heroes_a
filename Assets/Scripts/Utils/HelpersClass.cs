using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Localization.Settings;

/// <summary>
/// A static class for general helpful methods
/// </summary>
public static class Helpers
{
    /// <summary>
    /// Destroy all child objects of this transform (Unintentionally evil sounding).
    /// Use it like so:
    /// <code>
    /// transform.DestroyChildren();
    /// </code>
    /// </summary>
    public static void DestroyChildren(this Transform t)
    {
        foreach (Transform child in t) UnityEngine.Object.Destroy(child.gameObject);
    }

    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    /// <summary>
    /// Vector 3 to Matrix4x4
    /// </summary>
    /// <param name="input">Vector3</param>
    /// <returns>Vector3</returns>
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);

    public static string GetStringNameCountWarrior(int n)
    {
        string name = "few";
        switch (n)
        {
            case int i when n > 5 && n <= 9:
                name = "several";
                break;
            case int i when n > 9 && n <= 19:
                name = "pack";
                break;
            case int i when n > 19 && n <= 49:
                name = "lots";
                break;
            case int i when n > 49 && n <= 99:
                name = "horde";
                break;
            case int i when n > 99 && n <= 249:
                name = "throng";
                break;
            case int i when n > 249 && n <= 499:
                name = "swarm";
                break;
            case int i when n > 499 && n <= 999:
                name = "cloud";
                break;
            case int i when n > 999:
                name = "legion";
                break;
        }

        return string.Format("army_{0}", name);
    }
}
public static class HelperLanguage
{
    public static LangItem GetLocaleText(this ScriptableUnitBase unit)
    {
        var t = unit.Locale.Find(t => t.Language == LocalizationSettings.SelectedLocale);
        return t;
    }

}

public interface IProperty<T> : IProperty
{
    new event Action<T> ValueChanged;
    new T Value { get; }
}

public interface IProperty
{
    event Action<object> ValueChanged;
    object Value { get; }
}

[Serializable]
public class Property<T> : IProperty<T>
{
    public event Action<T> ValueChanged;

    event Action<object> IProperty.ValueChanged
    {
        add => valueChanged += value;
        remove => valueChanged -= value;
    }

    [SerializeField]
    private T value;

    public T Value
    {
        get => value;

        set
        {
            if (EqualityComparer<T>.Default.Equals(this.value, value))
            {
                return;
            }

            this.value = value;

            ValueChanged?.Invoke(value);
            valueChanged?.Invoke(value);
        }
    }

    object IProperty.Value => value;

    private Action<object> valueChanged;

    public Property(T value) => this.value = value;

    public static explicit operator Property<T>(T value) => new Property<T>(value);
    public static implicit operator T(Property<T> binding) => binding.value;
}