import { initializeApp } from "https://www.gstatic.com/firebasejs/10.8.0/firebase-app.js";
import { getAuth, signInWithEmailAndPassword } from "https://www.gstatic.com/firebasejs/10.8.0/firebase-auth.js";

// La configuración exacta de tu proyecto
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

document.getElementById("btnIngresar").addEventListener("click", async (e) => {
    e.preventDefault(); // Evita que la página se recargue sola

    const email = document.getElementById("loginEmail").value;
    const password = document.getElementById("loginPassword").value;
    const errorDiv = document.getElementById("mensajeErrorLogin");

    errorDiv.style.display = "none";

    // Pequeña validación antes de ir a Google
    if (!email || !password) {
        errorDiv.innerText = "Por favor, completa todos los campos.";
        errorDiv.style.display = "block";
        return;
    }

    try {
        // 1. Firebase verifica el correo y la contraseña
        const userCredential = await signInWithEmailAndPassword(auth, email, password);

        // 2. Extraemos el comprobante (Token)
        const token = await userCredential.user.getIdToken();

        // 3. Enviamos el token a tu controlador C#
        const response = await fetch('/Cuenta/SincronizarLoginWeb', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ Token: token })
        });

        if (response.ok) {
            const data = await response.json();
            // 4. Sesión creada en C#, viajamos al inicio
            window.location.href = data.urlRedireccion;
        } else {
            errorDiv.innerText = "La cuenta no existe en nuestra base de datos. Por favor, regístrate.";
            errorDiv.style.display = "block";
        }
    } catch (error) {
        // Si Google dice que la clave está mal
        errorDiv.innerText = "Correo o contraseña incorrectos.";
        errorDiv.style.display = "block";
    }
});