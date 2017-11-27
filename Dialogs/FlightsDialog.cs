namespace MultiDialogsBot.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class FlightsDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            // context.Fail(new NotImplementedException("Flights Dialog is not implemented and is instead being used to show context.Fail"));
            await context.PostAsync("List of Discount Products are...\n\n1.Water Bottle\n\n2.Cigarettee");
            await context.PostAsync("Please select your product name..");
            context.Wait(this.DiscoutProducts1);

        }

        public virtual async Task DiscoutProducts1(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            //int no = Convert.ToInt32(activity.Text);
            if (activity.Text.ToLower().Contains("water bottle"))
            {
                //await context.Forward(new NewOrderDialog(), this.ResumeAfterNewOrderDialog, message, CancellationToken.None);
                await context.PostAsync($"Ok, Cost of the {activity.Text} is Rs.20");
                await context.PostAsync($"After discount the {activity.Text} price is Rs.15");
                await context.PostAsync("Are you sure to purchase it...?(Yes or No)");
                context.Wait(this.Confirmation123); 
            }
            else if (activity.Text.ToLower().Contains("cigarettee"))
            {
                await context.PostAsync($"Ok, Cost of the {activity.Text} is Rs.15");
                await context.PostAsync($"After discount the {activity.Text} price is Rs.12");
                await context.PostAsync("Are you sure to purchase it...?(Yes or No)");
                context.Wait(this.Confirmation123);
            }
            else
            {
                await context.PostAsync("Please selct the products available in list...");
                context.Wait(DiscoutProducts1);
            }
        }


        private  async Task Confirmation123(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            if (activity.Text.ToLower().Contains("yes"))
            {
                await context.PostAsync("Ok, your order will be confirmed...our team will consult u for further details");
            }
            else if(activity.Text.ToLower().Contains("no"))
            {
                await context.PostAsync("Thank you...visit again");
            }
            context.Wait(this.FinalDialog);
        }

        private async Task FinalDialog(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Thanks for purchasing...");
        }

    }
}