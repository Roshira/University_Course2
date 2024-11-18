#include <iostream>
#include <vector>
#include <string>

using namespace std;

vector<int> buildKMPTable(const string& pattern) {
    int m = pattern.length();
    vector<int> lps(m, 0);
    int j = 0;

    for (int i = 1; i < m; ++i) {
        while (j > 0 && pattern[i] != pattern[j]) {
            j = lps[j - 1];
        }
        if (pattern[i] == pattern[j]) {
            ++j;
        }
        lps[i] = j;
    }
    return lps;
}

bool KMP(const string& text, const string& pattern) {
    int n = text.length();
    int m = pattern.length();

    vector<int> lps = buildKMPTable(pattern);
    int j = 0;
    for (int i = 0; i < n; ++i) {
        while (j > 0 && text[i] != pattern[j]) {
            j = lps[j - 1];
        }
        if (text[i] == pattern[j]) {
            ++j;
        }
        if (j == m) {
            return true; 
        }
    }
    return false;
}

bool isCyclicShift(const string& T, const string& T_star) {
    if (T.length() != T_star.length()) {
        return false;
    }

    string doubledT = T + T;

    return KMP(doubledT, T_star);
}

int main() {
    string T, T_star;

    cout << "Enter row T: ";
    cin >> T;

    cout << "Enter row T*: ";
    cin >> T_star;

    if (isCyclicShift(T, T_star)) {
        cout << "Yes, it is the cyclic shift of the string T.\n";
    }
    else {
        cout << "Not, it is not the cyclic shift of the string T.\n";
    }

    return 0;
}
