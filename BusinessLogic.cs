using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Communication.Email;
using Azure.Communication.Email.Models;

namespace FinalProject
{
    class BusinessLogic
    {
    
        static async Task Main(string[] args)
        {
            bool _continue = true;
            Staff staff;
            GuiTier appGUI = new GuiTier();
            DataTier database = new DataTier();

            // start GUI
            staff = appGUI.Login();

        
            if (database.LoginCheck(staff)){

                while(_continue){
                    int option  = appGUI.Options(staff);
                    string target_email = "";
                    switch(option)
                    {
                        // Search Resident
                        case 1:
                            Console.WriteLine("Please input Resident full name:");
                            string full_name = Console.ReadLine();
                            DataTable residentTable = database.getResident(full_name);
                            if(residentTable != null)
                                target_email = appGUI.DisplayResidents(residentTable);
                                database.SearchResident(full_name);
                                if(target_email != null){
                                Console.WriteLine($"Target Email: {target_email}");
                                // send email to target resident
                                 string serviceConnectionString =  "endpoint=https://crbaca1communicationservice.communication.azure.com/;accesskey=P6aGIlSgzEvOIDlwhBL/XsZc63ddC+D/mf4EhmirGShsSQaabnblXIJ14qk8Or0vwcCwdmZGksv1nUp+Wm2/QA==";
                                    EmailClient emailClient = new EmailClient(serviceConnectionString);
                                    var subject = "Hello CIDM4360/5360 Week10";
                                    var emailContent = new EmailContent(subject);
                                    // use Multiline String @ to design html content
                                    emailContent.Html= @"
                                                <html>
                                                    <body>
                                                        <h1 style=color:red>Testing Email for Azure Email Service</h1>
                                                        <h4>Hello,</h4>
                                                        <p>We have received a packagein the office for you will have 5 days to pick up your package. Or it will be sent back with the post office.</p>
                                                        <p>Thank you!</p>
                                                    </body>
                                                </html>";


                                    // mailfrom domain of your email service on Azure
                                    var sender = "DoNotReply@31a2f66e-aec7-4d9a-8088-fe0ad0cd25d6.azurecomm.net";

                                    
                                    string inputEmail = target_email;
                                    var emailRecipients = new EmailRecipients(new List<EmailAddress> {
                                        new EmailAddress(inputEmail) { DisplayName = "Testing" },
                                    });

                                    var emailMessage = new EmailMessage(sender, emailContent, emailRecipients);

                                    try
                                    {
                                        SendEmailResult sendEmailResult = emailClient.Send(emailMessage);

                                        string messageId = sendEmailResult.MessageId;
                                        if (!string.IsNullOrEmpty(messageId))
                                        {
                                            Console.WriteLine($"Email sent, MessageId = {messageId}");
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Failed to send email.");
                                            return;
                                        }

                                        // wait max 2 minutes to check the send status for mail.
                                        var cancellationToken = new CancellationTokenSource(TimeSpan.FromMinutes(2));
                                        do
                                        {
                                            SendStatusResult sendStatus = emailClient.GetSendStatus(messageId);
                                            Console.WriteLine($"Send mail status for MessageId : <{messageId}>, Status: [{sendStatus.Status}]");

                                            if (sendStatus.Status != SendStatus.Queued)
                                            {
                                                break;
                                            }
                                            await Task.Delay(TimeSpan.FromSeconds(10));
                                            
                                        } while (!cancellationToken.IsCancellationRequested);

                                        if (cancellationToken.IsCancellationRequested)
                                        {
                                            Console.WriteLine($"Looks like we timed out for email");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Error in sending email, {ex}");
                                    }
                                }
                                    
                            break;
                                
                            case 2:
                                Console.WriteLine("Please input Resident full name:");
                                string full_name1 = Console.ReadLine();
                                bool pickedUp = database.processPackage(full_name1);
                                if(pickedUp){
                                    Console.WriteLine("package Process");
                                }

                            break;
                            case 3:
                                Console.WriteLine("Please input Resident full name:");
                                string full_name2 = Console.ReadLine();
                                bool returned = database.returnedPackage(full_name2);
                                if(returned){
                                    Console.WriteLine("returned");
                                }
                            break;
                             case 4:
                                Console.WriteLine("Please input Resident full name:");
                                string full_name3 = Console.ReadLine();
                                DataTable history = database.retrieveHistory(full_name3);
                                foreach(DataRow row in history.Rows){
                                    Console.WriteLine($" FullName: {row["full_name"]} \t Agency:{row["agency"]} \t Status:{row["Status"]}");
                                }
                            break;

                         
                    }
                    
                }

            }
            else{
                    Console.WriteLine("Login Failed, Try again.");
                }  
        }      
    }
}






