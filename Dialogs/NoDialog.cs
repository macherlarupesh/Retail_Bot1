namespace MultiDialogsBot.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class NoDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            // context.Fail(new NotImplementedException("Flights Dialog is not implemented and is instead being used to show context.Fail"));
            await context.PostAsync("How can I help you..?");
            context.Wait(this.Message32);

        }


        public virtual async Task Message32(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
           // if (activity.Text.ToLower().Contains("list of products"))
            //{
                await context.PostAsync("Yes");
                await context.PostAsync("List of Products are...\n\n1.Pizza\n\n2.Burger\n\n3.Biscuits");
                await context.PostAsync("Please select your product name..");
                context.Wait(this.Products1);
           // }
        }

        public virtual async Task Products1(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (activity.Text.ToLower().Contains("pizza"))
            {
                //await context.Forward(new NewOrderDialog(), this.ResumeAfterNewOrderDialog, message, CancellationToken.None);
                await context.PostAsync($"Ok, Cost of the {activity.Text} is Rs.250");
                await context.PostAsync("Are you sure to purchase it...?(Yes or No)");
                context.Wait(this.Confirmation123);
            }
            else if (activity.Text.ToLower().Contains("burger"))
            {
                await context.PostAsync($"Ok, Cost of the {activity.Text} is Rs.100");
                await context.PostAsync("Are you sure to purchase it...?(Yes or No)");
                context.Wait(this.Confirmation123);
            }
            else if (activity.Text.ToLower().Contains("biscuits"))
            {
                await context.PostAsync($"Ok, Cost of the {activity.Text} is Rs.35");
                await context.PostAsync("Are you sure to purchase it...?(Yes or No)");
                context.Wait(this.Confirmation123);
            }
            else
            {
                await context.PostAsync("Please selct the products available in list...");
                context.Wait(Products1);
            }
        }


        private async Task Confirmation123(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            if (activity.Text.ToLower().Contains("yes"))
            {
                await context.PostAsync("Ok, your order will be confirmed...our team will consult u for further details");
            }
            else if (activity.Text.ToLower().Contains("no"))
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