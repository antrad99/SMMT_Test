using Microsoft.AspNetCore.Components;
using Radzen;
using SMMT_Test.Services;
using SMMT_Test.Dtos;


namespace SMMT_Test.Components.Pages
{
    public partial class AddFeedback
    {
        [Inject]
        private IFeedbackService feedbackService { get; set; }

        [Inject]
        private DialogService dialogService { get; set; }

        private FeedbackDto model = new FeedbackDto();


        async Task OnSubmit()
        {
            await feedbackService.AddFeedback(model);


            dialogService.Close(model);
        }
    }
}
