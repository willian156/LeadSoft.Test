using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LeadSoft.Test.DAO;
using LeadSoft.Test.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using LeadSoft.Test.Hubs;
using LeadSoft.Test.Models.DTO.Anime;
using LeadSoft.Test.Commom.Enums;

namespace LeadSoft.Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimesController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<ProcessingDataHub> _hubContext;
        private readonly DataContext _context;

        public AnimesController(DataContext context, IServiceProvider serviceProvider, IHubContext<ProcessingDataHub> hubContext)
        {
            _context = context;
            _serviceProvider = serviceProvider;
            _hubContext = hubContext;
        }

        // GET: api/Animes
        [HttpGet("{UserId}")]
        public async Task<ActionResult<IEnumerable<Anime>>> GetAnimes(int UserId)
        {
            try
            {
                return await _context.Animes.Where<Anime>(x => x.User.Id == UserId).ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("StartProcessing/{AdminId}")]
        public async Task<IActionResult> StartProcessing(int AdminId, ProcessRequestDTO process)
        {
            try
            {
                var verifyRole = _context.Users.Find(AdminId).Role;
                var getUser = _context.Users.Find(process.UserId);

                if (verifyRole == RolesEnum.Admin)
                {


                    string groupId = $"{process.UserId}_{getUser.Username}";

                    var usr = _context.Users.FirstOrDefault(x => x.Id == process.UserId);


                    CancellationTokenSource cts = new CancellationTokenSource();
                    ProcessingTasksManager.AddTask(groupId, cts);


                    Task.Run(async () =>
                    {

                        using (var scope = _serviceProvider.CreateScope())
                        {



                            string csvFilePath = "./Archive/final_animedataset.csv";

                            if (!System.IO.File.Exists(csvFilePath))
                            {
                                await _hubContext.Clients.Group(groupId).SendAsync("ProcessingError", "Arquivo CSV não encontrado.");
                                return;
                            }


                            using (var reader = new StreamReader(csvFilePath))
                            {
                                int validLines = 0;
                                await _hubContext.Clients.Group(groupId)
                                   .SendAsync("ProcessingStarted", groupId);
                                while (!reader.EndOfStream && !cts.Token.IsCancellationRequested)
                                {
                                    var line = await reader.ReadLineAsync();

                                    var columns = line.Split(',');

                                    if (columns[6].ToString().Contains(process.Type) && columns[7].ToString().Contains(process.Source))
                                    {
                                        validLines++;

                                        var processedItem = new Anime
                                        {
                                            Gender = columns[4],
                                            Title = columns[5],
                                            Type = columns[6],
                                            Source = columns[7],
                                            User = usr

                                        };

                                        _context.Animes.Add(processedItem);
                                        await _context.SaveChangesAsync(cts.Token);

                                        await _hubContext.Clients.Group(groupId)
                                            .SendAsync("ProcessingUpdate", new { ProcessedLines = validLines, ProcessedItem = processedItem });
                                    }
                                }
                            }

                            if (cts.Token.IsCancellationRequested)
                            {
                                await _hubContext.Clients.Group(groupId)
                                    .SendAsync("ProcessingCanceled", groupId);
                            }
                            else
                            {
                                await _hubContext.Clients.Group(groupId)
                                    .SendAsync("ProcessingCompleted", groupId);
                            }
                        }

                        ProcessingTasksManager.RemoveTask(groupId);
                    }, cts.Token).Wait();

                    return Ok(new { message = "Processamento Terminado" });
                }
                else
                {
                    return Unauthorized(new { message = "Você não tem permissão para iniciar o processamento!" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }



        [HttpPost("CancelProcessing")]
        public async Task<IActionResult> CancelProcessing(string groupId)
        {
            try
            {
                if (ProcessingTasksManager.TryGetTask(groupId, out var cts))
                {
                    cts.Cancel();
                    ProcessingTasksManager.RemoveTask(groupId);
                    await _hubContext.Clients.Group(groupId)
                                .SendAsync("ProcessingCanceled", groupId);
                    return Ok(new { message = "Processamento cancelado" });
                }

                return NotFound(new { message = "Processamento não encontrado ou já finalizado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
    }
}
