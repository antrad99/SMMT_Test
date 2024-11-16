using Microsoft.AspNetCore.Components;
using Radzen;
using SMMT_Test.Dtos;
using SMMT_Test.Services;

namespace SMMT_Test.Components.Pages
{
    public partial class EditFeedback
    {
        [Parameter]
        public FeedbackDto feedback { get; set; }

        FeedbackDto model = new FeedbackDto();

        [Inject]
        private IFeedbackService feedbackService { get; set; }
        [Inject]
        private DialogService dialogService { get; set; }

        protected override void OnParametersSet()
        {
            var properties = typeof(FeedbackDto).GetProperties();
            foreach (var property in properties)
            {
                property.SetValue(model, property.GetValue(feedback));
            }
        }


        async Task OnSubmit()
        {
            await feedbackService.UpdateFeedback(model);


            dialogService.Close(model);
        }
    }
}
