using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MVC5Course.Models;
using MVC5Course.ViewModels;

namespace MVC5Course.Controllers
{
    [RoutePrefix("clients")]
    public class ClientsController : BaseController
    {
        ClientRepository repo;
        OccupationRepository repoOcc;
        public ClientsController()
        {
            repo = RepositoryHelper.GetClientRepository();
            repoOcc = RepositoryHelper.GetOccupationRepository(repo.UnitOfWork);
        }

        // GET: Clients
        [Route("")]
        public ActionResult Index()
        {
            var client = repo.All().Take(30).Include(c => c.Occupation);
            return View(client.ToList());
        }

        [HttpPost]
        [Route("BatchUpdate")]
        [HandleError(ExceptionType = typeof(DbEntityValidationException), View = "Error_DbEntityValidationException")]
        //public ActionResult BatchUpdate(IList<Client> data)   //也可以這樣寫
        public ActionResult BatchUpdate(ClientBatchViewModel[] data)
        {
            //這邊在找data的資料時會是這樣的樣子
            //data[0].ClientId
            //那麼View當中的設計name要成為這個格式，Modelbinding就可以binding到
            //所以在view中要將data轉為陣列

            if (ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    var client = repo.Find(item.ClientId);
                    client.FirstName = item.FirstName;
                    client.MiddleName = item.MiddleName;
                    client.LastName = item.LastName;
                }

                repo.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }

            ViewData.Model = repo.All().Take(30);

            return View("Index");
        }

        [Route("search")]
        [HttpGet]
        public ActionResult Search(string FirstName)
        {
            var client = repo.SearchFirstName(FirstName);
            return View("Index", client);
        }

        [Route("details/{id}")]
        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Client client = repo.Find(id.Value);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        [Route("{*name}")]
        public ActionResult Details2(string name)
        {
            string[] names = name.Split('/');
            string FirstName = names[0];
            string MiddleName = names[1];
            string LastName = names[2];

            Client client = repo.All().FirstOrDefault(c => c.FirstName == FirstName
            && c.MiddleName == MiddleName && c.LastName == LastName);

            if (client == null)
            {
                return HttpNotFound();
            }
            return View("Details", client);
        }

        // GET: Clients/Create
        [Route("create")]
        public ActionResult Create()
        {
            ViewBag.OccupationId = new SelectList(repoOcc.All(), "OccupationId", "OccupationName");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public ActionResult Create([Bind(Include = "ClientId,FirstName,MiddleName,LastName,Gender,DateOfBirth,CreditRating,XCode,OccupationId,TelephoneNumber,Street1,Street2,City,ZipCode,Longitude,Latitude,Notes,IdNumber")] Client client)
        {
            if (ModelState.IsValid)
            {
                repo.Add(client);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.OccupationId = new SelectList(repoOcc.All(), "OccupationId", "OccupationName", client.OccupationId);
            return View(client);
        }

        // GET: Clients/Edit/5
        [Route("edit/{id}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = repo.Find(id.Value);
            if (client == null)
            {
                return HttpNotFound();
            }
            ViewBag.OccupationId = new SelectList(repoOcc.All(), "OccupationId", "OccupationName", client.OccupationId);
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit/{id}")]
        //參數form只是為了與上方的Edit不衝突，並沒有要使用。
        public ActionResult Edit(int id,FormCollection form)
        {
            //取得這筆的所有資料
            var client = repo.Find(id);

            //這邊才做ModelBinding     
            //,prefix 這個就是設定欄位名稱必須是 設定的prefix.xxx欄位名稱
            //,includeProperties 
            //,valueProvider 會在取得client本來的資料，讓FirstName達成不被編輯的功能。
            if (TryUpdateModel(client,"",null, new string[] { "FirstName"}))
            {
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.OccupationId = new SelectList(repoOcc.All(), "OccupationId", "OccupationName", client.OccupationId);
            return View(client);
        }

        // GET: Clients/Delete/5
        [Route("delete/{id}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = repo.Find(id.Value);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("delete/{id}")]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = repo.Find(id);
            repo.Delete(client);
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
