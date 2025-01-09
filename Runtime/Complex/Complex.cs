
using UnityEngine;

public class Complex
{
    public float Real { get; set; }
    public float Imaginary { get; set; }

    public float Amplitude => Mathf.Sqrt(Real * Real + Imaginary * Imaginary);

    public float Phase => Mathf.Atan2(Imaginary, Real);

    public Complex(float real, float imaginary)
    {
        Real = real;
        Imaginary = imaginary;
    }

    public static Complex Polar(float r, float theta)
    {
        float real = r * Mathf.Cos(theta);
        float imaginary = r * Mathf.Sin(theta);
        return new Complex(real, imaginary);
    }

    public Complex Conjugate()
    {
        return new Complex(this.Real, this.Imaginary);
    }

    /*
    *   Adding
    */
    public static Complex operator +(Complex c1, Complex c2)
    {
        return new Complex(c1.Real + c2.Real, c1.Imaginary + c2.Imaginary);
    }

    /*
    *   Subtracting
    */
    public static Complex operator -(Complex c1, Complex c2)
    {
        return new Complex(c1.Real - c2.Real, c1.Imaginary - c2.Imaginary);
    }

    /*
    *  Multiplying
    */
    public static Complex operator *(Complex c1, Complex c2)
    {
        float real = c1.Real * c2.Real - c1.Imaginary * c2.Imaginary;
        float imaginary = c1.Real * c2.Imaginary + c1.Imaginary * c2.Real;
        return new Complex(real, imaginary);
    }

    public static Complex operator *(float scalar, Complex c)
    {
        return new Complex(c.Real * scalar, c.Imaginary * scalar);
    }

    public static Complex operator *(Complex c, float scalar)
    {
        return scalar * c;
    }

    /*
    *  Dividing
    */

    public static Complex operator /(float scalar, Complex c)
    {
        return new Complex(c.Real / scalar, c.Imaginary / scalar);
    }

    public static Complex operator /(Complex c, float scalar)
    {
        return scalar / c;
    }

    public override string ToString()
    {
        return $"{Mathf.Round(Real * 10000) / 10000.0f} + {Mathf.Round(Imaginary * 10000) / 10000.0f}i";
    }

    public void PrintPhaseAndAmplitude()
    {
        Debug.Log($"Amplitude: {Amplitude:F5}, Phase: {Phase:F5} radians");
    }

    public static Complex[] FromFloatArray(float[] floatArray)
    {
        Complex[] complexArray = new Complex[floatArray.Length];
        for (int i = 0; i < floatArray.Length; i++)
        {
            complexArray[i] = new Complex(floatArray[i], 0);
        }
        return complexArray;
    }
}
