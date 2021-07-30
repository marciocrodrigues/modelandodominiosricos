using Microsoft.AspNetCore.Mvc;
using System;

namespace NerdStore.WebApp.API.Controllers
{
    public abstract class ControllerBaseVendas : ControllerBase
    {
        // Para simular um cliente logado
        protected Guid ClienteId = Guid.Parse("5624e062-c2bb-45c0-838b-a427c19bba5b");
    }
}
