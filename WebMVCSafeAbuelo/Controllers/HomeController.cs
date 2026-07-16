using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebMVCSafeAbuelo.Models;

// 1. Agregamos los nuevos usings necesarios
using WebMVCSafeAbuelo.Services;
using WebMVCSafeAbuelo.ViewModels;
using WebMVCSafeAbuelo.Models.Enums;

namespace WebMVCSafeAbuelo.Controllers
{
    public class HomeController : Controller
    {
        // 2. Declaramos el servicio
        private readonly IIncidenteService _incidenteService;

        // 3. Inyectamos el servicio a través del constructor
        public HomeController(IIncidenteService incidenteService)
        {
            _incidenteService = incidenteService;
        }

        // 4. Modificamos el Index para que sea asíncrono y llene el ViewModel
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Index()
        {
            var todosLosReportes = await _incidenteService.ObtenerTodosAsync();

            var viewModel = new HomeIndexViewModel
            {
                TotalReportes = todosLosReportes.Count(),
                ReportesPendientes = todosLosReportes.Count(r => r.Estado == EstadoReporte.Pendiente),
                FraudesConfirmados = todosLosReportes.Count(r => r.Estado == EstadoReporte.Aceptado),

                AlertasRecientes = todosLosReportes
                    .Where(r => r.Estado == EstadoReporte.Aceptado)
                    .OrderByDescending(r => r.FechaReporte)
                    .Take(4)
            };

            return View(viewModel);
        }

        // =========================================================
        // TUS MÉTODOS ORIGINALES SE MANTIENEN EXACTAMENTE IGUAL
        // =========================================================

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            // Mensajes por defecto
            ViewBag.ErrorTitle = "¡Ups! Algo salió mal.";
            ViewBag.ErrorMessage = "Ha ocurrido un error inesperado al procesar tu solicitud.";

            // 1. Interceptamos el error exacto que hizo explotar la aplicación
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature?.Error;

            // 2. Evaluamos los errores de estado HTTP (Como 404 o 403)
            if (statusCode.HasValue)
            {
                if (statusCode.Value == 404)
                {
                    ViewBag.ErrorTitle = "Error 404 - Objetivo No Encontrado";
                    ViewBag.ErrorMessage = "El reporte, la evidencia o la página que buscas no existe o fue movida.";
                }
                else if (statusCode.Value == 403)
                {
                    ViewBag.ErrorTitle = "Error 403 - Acceso Denegado";
                    ViewBag.ErrorMessage = "Sus credenciales no tienen el nivel de autorización requerido para esta acción.";
                }
            }

            // 3. Evaluamos las Excepciones Internas (Error 500 y Bases de Datos)
            if (exception != null)
            {
                // A) Errores de Base de Datos (Neon dormido, desconexión de red, timeout)
                if (exception is DbUpdateException ||
                    exception.GetType().Name.Contains("NpgsqlException") ||
                    exception.GetType().Name.Contains("SocketException") ||
                    exception.InnerException?.GetType().Name.Contains("SocketException") == true)
                {
                    ViewBag.ErrorTitle = "Error de Conexión a la Base de Datos";
                    ViewBag.ErrorMessage = "El servidor de base de datos tardó mucho en responder o se encuentra suspendido por inactividad. Por favor, espera unos segundos y vuelve a intentarlo.";
                }
                // B) Errores de Concurrencia (Dos analistas editando el mismo reporte al mismo tiempo)
                else if (exception is DbUpdateConcurrencyException)
                {
                    ViewBag.ErrorTitle = "Conflicto de Edición";
                    ViewBag.ErrorMessage = "El registro que intentas guardar acaba de ser modificado o eliminado por otro usuario. Por favor, recarga la página.";
                }
                // C) Falla interna genérica
                else
                {
                    ViewBag.ErrorTitle = "Error 500 - Falla del Servidor";
                    ViewBag.ErrorMessage = "Nuestros sistemas están experimentando dificultades técnicas inesperadas. El evento ha sido registrado para su análisis.";
                }
            }

            return View(new WebMVCSafeAbuelo.Models.ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}