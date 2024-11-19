#include <iostream>
#include <complex>

using namespace std;

typedef complex<double> Complex;

Complex** allocateMatrix(int n) {
    Complex** matrix = new Complex * [n];
    for (int i = 0; i < n; i++) {
        matrix[i] = new Complex[n];
    }
    return matrix;
}

void deallocateMatrix(Complex** matrix, int n) {
    for (int i = 0; i < n; i++) {
        delete[] matrix[i];
    }
    delete[] matrix;
}

// ������� ��� ��������� �������
void printMatrix(Complex** matrix, int n, string name) {
    cout << "Matrix " << name << ":" << endl;
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < n; j++) {
            cout << matrix[i][j] << " ";
        }
        cout << endl;
    }
}

// ��������� ������ ������-������� ��� ����������� �������� �������
void gaussJordanInverse(Complex** A, Complex** I, int n) {
    for (int i = 0; i < n; i++) {
        // ���������� ��������� �������� �� �������
        Complex diagElem = A[i][i];
        for (int j = 0; j < n; j++) {
            A[i][j] /= diagElem;
            I[i][j] /= diagElem;
        }

        // ���������� ����� �������� � ������� �� ����
        for (int k = 0; k < n; k++) {
            if (k != i) {
                Complex factor = A[k][i];
                for (int j = 0; j < n; j++) {
                    A[k][j] -= factor * A[i][j];
                    I[k][j] -= factor * I[i][j];
                }
            }
        }
    }
}

int main() {
    int n;
    cout << "Enter the matrix size: ";
    cin >> n;

    Complex** A = allocateMatrix(n);
    Complex** I = allocateMatrix(n);

    // ���������� ������� A � �������� ������� I
    cout << "Enter the matrix elements (real and imaginary part):" << endl;
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < n; j++) {
            double realPart, imaginaryPart;
            cout << "A[" << i << "][" << j << "] (real part): ";
            cin >> realPart;
            cout << "A[" << i << "][" << j << "] (imaginary part): ";
            cin >> imaginaryPart;
            A[i][j] = Complex(realPart, imaginaryPart);
        }
        I[i][i] = Complex(1, 0); // ����������� �������� �������
    }

    // ������������ ������ ������-�������
    gaussJordanInverse(A, I, n);

    // ��������� �������� �������
    printMatrix(I, n, "A^-1");

    // �������� ���'��
    deallocateMatrix(A, n);
    deallocateMatrix(I, n);

    return 0;
}
