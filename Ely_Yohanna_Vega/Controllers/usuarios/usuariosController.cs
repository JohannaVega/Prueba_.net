using System;

using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Ely_Yohanna_Vega.Models.db_datos;
using Ely_Yohanna_Vega.Models;
using System.Net.Mail;
using System.IO;

namespace Ely_Yohanna_Vega.Controllers.db_datos
{
    public class usuariosController : Controller
    {
        private db_datosEntities db = new db_datosEntities();

        // GET: usuarios, Nos muestra la lista de usuarios
        public ActionResult Index()
        {
            return View(db.usuarios.ToList());
        }

        // GET: usuarios/Details, Nos muestra la información de un usuario
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuarios usuarios = db.usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // GET: usuarios/Create, Nos muestra la vista que permite registrar datos
        public ActionResult Create()
        {
            return View();
        }

        // POST: usuarios/Create, Recibimos el archivo y lo guardamos en la BD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FileViewModel file)
        {
            String correodef = "";

            //Validando que se haya cargado correctamente
            if (!ModelState.IsValid)
            {
                //Si hay error redirecciono al create con el archivo ingresado
                return View("Create", file);
            }

            int i = 0;
            //Sección lectura del archivo y posterior insercion de datos a la bd
            using (StreamReader fielRead = new StreamReader(file.archivo.InputStream))
            {
                string line = null;
                String content = "";
                while ((line = fielRead.ReadLine()) != null)
                {
                   
                    if (i>0)//ignoro la primera fila
                    {
                        usuarios user = new usuarios();
                        var datos = line.Split(';');
                        String nam = datos[0].ToString();
                        String ape = datos[1].ToString();
                        String iden = datos[2].ToString();
                        String cel = datos[3].ToString();
                        String dir = datos[4].ToString();
                        String ciu = datos[5].ToString();
                        String corr = datos[6].ToString();

                        if (corr.Equals("Carlos.perez@pagosinteligentes.com"))
                        {
                            correodef = corr;
                        }
                        //Asigno los datos al objeto usuario
                        user.nombres = nam;
                        user.apellidos = ape;
                        user.identificacion = iden;
                        user.celular = cel;
                        user.direccion = dir;
                        user.cuidad = ciu;
                        user.correo = corr;

                        try
                        {
                            db.usuarios.Add(user);
                            db.SaveChanges();
                            content = "Exitoso: Datos almacenados correctamente del usuario Pagos Inteligentes";
                            enviar_email(correodef,content);
                        }
                        catch (Exception)
                        {
                            content = "Fallido: Falló al almacenar los datos del usuario Pagos Inteligentes";
                            enviar_email(correodef, content);
                        }
                       

                    }
                    i++;

                }
                

            }
            return RedirectToAction("Index");

        }

        //Función que Envia e-mail durante el proceso de inserción de datos
        public void enviar_email(String correodef, String content)
        {
            MailAddress from = new MailAddress("eyvegaf@gmail.com");//Remitente
            MailAddress to = new MailAddress(correodef);//destinatario
            MailMessage mensaje = new MailMessage(from, to);

            mensaje.Subject = "Estado proceso de inserción datos";
            mensaje.SubjectEncoding = System.Text.Encoding.UTF8;//Ecoding el formato del mensaje
            mensaje.Bcc.Add("eyvegaf@gmail.com");//correo de copia
            mensaje.Body = content;
            mensaje.BodyEncoding = System.Text.Encoding.UTF8;//Ecoding el formato del mensaje
            mensaje.IsBodyHtml = true;
            
            SmtpClient cliente = new SmtpClient();//Protocolo Smtp


            //CREDENCIALES CORREO REMITENTE
            cliente.UseDefaultCredentials = false;
            cliente.Credentials = new NetworkCredential(correodef, "123J M*1");
            cliente.Port = 587;
            cliente.EnableSsl = true;
            //SERVIDOR DE SALIDA DE GMAIL
            cliente.Host = "smtp.gmail.com";

            try
            {
                cliente.Send(mensaje);
                cliente.Dispose();

            }
            catch (Exception)
            {
               Console.WriteLine("Error al enviar el correo");

            }
        }

        // GET: usuarios/Edit, Metodo nos muestra la vista para editar datos
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuarios usuarios = db.usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // POST: usuarios/Edit, , Metodo que permite editar datos por el id

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_usuario,nombres,apellidos,identificacion,celular,direccion,cuidad,correo")] usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuarios);
        }

        // GET: usuarios/Delete, Metodo que nos muestra la vista para confirmar delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuarios usuarios = db.usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // POST: usuarios/Delete, , Metodo que permite borrar usaurios por el id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            usuarios usuarios = db.usuarios.Find(id);
            db.usuarios.Remove(usuarios);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //Liberando recursos no administrados
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
