namespace SimpleCircuit {

    open Microsoft.Quantum.Canon;
    open Microsoft.Quantum.Intrinsic;
    

    operation simpleQsharp() : Unit {
        using (qubit = Qubit[4]) {
            X(qubit[0]);
            H(qubit[1]);
            Adjoint S(qubit[2]);
            R(PauliY, 1.234, qubit[2]);
            Adjoint Y(qubit[1]); 
            Adjoint R(PauliZ, 0.789, qubit[3]);
            CX(qubit[2], qubit[3]);
            CY(qubit[1], qubit[2]);
            CZ(qubit[3], qubit[0]);
            Controlled H([qubit[2]], qubit[3]);
            Controlled R([qubit[1]], (PauliX, 1.234, qubit[2]));
            Controlled R([qubit[1]], (PauliY, 1.234, qubit[2]));
            Controlled R([qubit[1]], (PauliZ, 1.234, qubit[2]));

            let res = M(qubit[3]);
        }
    }
}


