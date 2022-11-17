using EnvDTE;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NuGet.Protocol;
using Prueba2.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;


namespace Prueba2.Controllers
{
    public class TareasController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        HttpClient client;
        Uri baseadrres = new Uri("http://localhost:9095/api/Tareas");
        
        public TareasController()
        {


            client = new HttpClient();
            client.BaseAddress = baseadrres;

        }

        public IActionResult Index()
        {
            List<Cl_Tareas> Lista = new List<Cl_Tareas>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/get").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Lista = JsonConvert.DeserializeObject<List<Cl_Tareas>>(data);

            }
            return View(Lista);
        }

        
        




        public ActionResult Delete(int id, string Estado)
        {

            if (Estado == "En Proceso")
            {
                TempData["ErrorMessage"] = "La Tarea no se puede eliminar si esta en proceso";
                return RedirectToAction("Index");
            }


            else
            {
                HttpResponseMessage response = client.DeleteAsync(client.BaseAddress + "/delete/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "La Tarea se elimino con exito";
                    return RedirectToAction("Index");

                }
            }
               
                return View();
            
        }   


        public ActionResult Create()
        {
            List<Cl_Colaboradores> Lista = new List<Cl_Colaboradores>();
            HttpResponseMessage response = client.GetAsync("http://localhost:9095/api/Colaboradores/get").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Lista = JsonConvert.DeserializeObject<List<Cl_Colaboradores>>(data);

            }

            Lista.Insert(0, new Cl_Colaboradores { Id = 0, Nombre = "Seleccione un Colaborador" });


            ViewBag.ListofCategory = Lista;



            return View();

        }

        [HttpPost]
        public ActionResult Create( Cl_Tareas Tareas)
        {
            string data = JsonConvert.SerializeObject(Tareas);
            StringContent content = new StringContent(data,Encoding.UTF8,"application/json");
            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/post", content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["AlertMessage"] = "La Tarea se creo con exito";
                return RedirectToAction("Index");
            }


            return View();

        }



        public ActionResult Edit(int id , string Estado )
        {

            if (Estado == "Finalizada")
            {
                TempData["ErrorMessage"] = "La Tarea no se puede editar si esta finalizada";
                return RedirectToAction("Index");
            }


            else
            {
                Cl_Tareas Tareas = new Cl_Tareas();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Lista_ID/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    Tareas = JsonConvert.DeserializeObject<Cl_Tareas>(data);

                }

                List<Cl_Colaboradores> Lista = new List<Cl_Colaboradores>();
                HttpResponseMessage response1 = client.GetAsync("http://localhost:9095/api/Colaboradores/Lista_ID/" + Tareas.ColaboradorId).Result;
                if (response1.IsSuccessStatusCode)
                {
                    string data1 = response1.Content.ReadAsStringAsync().Result;
                    Lista = JsonConvert.DeserializeObject<List<Cl_Colaboradores>>(data1);

                }

                Lista.Insert(0, new Cl_Colaboradores { Id = Tareas.ColaboradorId, Nombre = Tareas.Colaborador });


                ViewBag.ListofCategory = Lista;

                return View("Edit", Tareas);
            }
          
            
        }

        [HttpPost]
        public ActionResult Edit(Cl_Tareas Tareas)
        {
            string data = JsonConvert.SerializeObject(Tareas);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(client.BaseAddress + "/put/"+Tareas.id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["AlertMessage"] = "La Tarea se edito con exito";
                return RedirectToAction("Index");
            }
            return View("Edit",Tareas);

        }


    }
}
