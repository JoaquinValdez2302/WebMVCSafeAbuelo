import { initializeApp } from "https://www.gstatic.com/firebasejs/10.8.0/firebase-app.js";
import { getAuth, createUserWithEmailAndPassword } from "https://www.gstatic.com/firebasejs/10.8.0/firebase-auth.js";

const firebaseConfig = {
    apiKey: "AIzaSyBi_gvm_PMvYbkpqscgnQhDzFnE99vUoj0",
    authDomain: "safeabuelo.firebaseapp.com",
    projectId: "safeabuelo",
    storageBucket: "safeabuelo.firebasestorage.app",
    messagingSenderId: "315731791091",
    appId: "1:315731791091:web:99ba733a3221903cdb7b86",
    measurementId: "G-K2JY31PXZQ"
};

const app = initializeApp(firebaseConfig);
const auth = getAuth(app);

document.getElementById("btnRegistrar").addEventListener("click", async () => {
    const nombre = document.getElementById("regNombre").value;
    const telefono = document.getElementById("regTelefono").value;
    const email = document.getElementById("regEmail").value;
    const password = document.getElementById("regPassword").value;
    const errorDiv = document.getElementById("mensajeError");

    errorDiv.style.display = "none";

    try {
        const userCredential = await createUserWithEmailAndPassword(auth, email, password);
        const token = await userCredential.user.getIdToken();

        const response = await fetch('/Cuenta/SincronizarRegistroWeb', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                Token: token,
                NombreCompleto: nombre,
                Telefono: telefono,
                Email: email
            })
        });

        if (response.ok) {
            const data = await response.json();
            window.location.href = data.urlRedireccion;
        } else {
            errorDiv.innerText = "Error al sincronizar con la base de datos.";
            errorDiv.style.display = "block";
        }
    } catch (error) {
        errorDiv.innerText = "Error de Firebase: " + error.message;
        errorDiv.style.display = "block";
    }
});