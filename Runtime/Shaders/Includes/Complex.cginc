#ifndef _COMPLEX_
#define _COMPLEX_

#define Complex float2
#define real r
#define imag g

Complex ComplexAdd(Complex a, Complex b) {
    return Complex(a.real + b.real, a.imag + b.imag);
}

Complex ComplexSub(Complex a, Complex b) {
    return Complex(a.real - b.real, a.imag - b.imag);
}

Complex ComplexMult(Complex a, Complex b) {
    return Complex(
        a.real * b.real - a.imag * b.imag,
        a.real * b.imag + a.imag * b.real
    );
}

Complex ComplexConjugate(Complex a) {
    return Complex(a.real, -a.imag);
}

float ComplexMagnitude(Complex a) {
    return sqrt(a.real * a.real + a.imag * a.imag);
}

float ComplexPhase(Complex a) {
    return atan2(a.imag, a.real);
}



#endif