namespace DaniBot.Dialog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.FormFlow;
    using Microsoft.Bot.Builder.Luis;
    using Microsoft.Bot.Builder.Luis.Models;
    using Microsoft.Bot.Connector;
    using Model;

    [Serializable]
    [LuisModel("7a0f5254-faf9-4ec8-9b31-703eac48e847", "5c14631880dd44bcb651b998b07ea7d4")]
    public class RootLuisDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            //string message = $"Sorry, I did not understand '{result.Query}'. Type 'help' if you need assistance.";
            string message = $"Lo siento, no le he entendido.";
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }
        [LuisIntent("Dani")]
        public async Task Dani(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Dani esta ahora mismo al telefono");
            await context.PostAsync("Si quiere puedo tomarle nota de su aviso");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Telefono")]
        public async Task Telefono(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Nuestro número de telefono es el 965 67 58 39");
            await context.PostAsync("Nuestro horario de atención telefonica es de 8:00 a 14:00 y de 16:00 a 18:30 de Lunes a Jueves");
            await context.PostAsync("Y los viernes de 8:00 a 14:00.");
            await context.PostAsync("Si desea dejarnos un mensaje para que le llamemos pulse aqui: http://www.informaticaros.com/avisos");
            context.Wait(MessageReceived);
        }
        [LuisIntent("Aviso")]
        public async Task Aviso(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;
            await context.PostAsync($"Voy a tomar nota de su incidencia, por favor contesteme a los siguientes datos:");
            var avisosForm = new AvisoForm();
            var hotelsFormDialog = new FormDialog<AvisoForm>(avisosForm, this.BuildHotelsForm, FormOptions.PromptInStart, result.Entities);
            context.Call(hotelsFormDialog, this.ResumeAfterHotelsFormDialog);
        }
        private IForm<AvisoForm> BuildHotelsForm()
        {
            OnCompletionAsyncDelegate<AvisoForm> processNuevoAviso = async (context, state) =>
            {
                var message = "Hemos tomado nota de su aviso, su número de aviso es XXX";
                await context.PostAsync(message);
            };
            return new FormBuilder<AvisoForm>()
                .Field(nameof(AvisoForm.Empresa), (state) => string.IsNullOrEmpty(state.Empresa))
                .Field(nameof(AvisoForm.Contacto), (state) => string.IsNullOrEmpty(state.Contacto))
                .Field(nameof(AvisoForm.Telefono), (state) => string.IsNullOrEmpty(state.Telefono))
                .Field(nameof(AvisoForm.Motivo), (state) => string.IsNullOrEmpty(state.Motivo))
                .OnCompletion(processNuevoAviso)
                .Build();
        }

        /*internal static IDialog<AvisoForm> MakeRootDialog()
        {
            return Chain.From(() => FormDialog.FromForm(AvisoForm.BuildForm));
        }*/
        private Task ResumeAfterHotelsFormDialog(IDialogContext context, IAwaitable<AvisoForm> result)
        {
            return null;
        }

    }
}