﻿using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SendEmailDailyLibrary
{
    public class DailyEmailSender
    {
        public event EventHandler<DailyEmailEventArgs> DailyEmailEvent;

        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _senderEmail;
        private readonly string _recipientEmail;
        private readonly string _subject;
        private readonly string _body;
        private readonly string _connectionString;

        public DailyEmailSender(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword, string senderEmail, string recipientEmail, string subject, string body, string connectionString)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUsername = smtpUsername;
            _smtpPassword = smtpPassword;
            _senderEmail = senderEmail;
            _recipientEmail = recipientEmail;
            _subject = subject;
            _body = body;
            _connectionString = connectionString;
        }

        public async Task SendDailyEmail()
        {
            try
            {
                // Retrieve data added in the last day from the database
                var lastDay = DateTime.Now;
                var newData = await GetDataAddedInLastDay(lastDay);

                if (newData.Any())
                {
                    // Trigger the event and notify subscribers
                    OnDailyEmailEvent(new DailyEmailEventArgs { NewData = newData });

                    // Send email with the new data
                    await SendEmail(newData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async Task<IEnumerable<YourEntity>> GetDataAddedInLastDay(DateTime lastDay)
        {
            using (IDbConnection dbConnection = new MySqlConnection(_connectionString))
            {
                dbConnection.Open();

                var query = "SELECT * FROM YourTable WHERE CreatedAt >= @LastDay";
                var newData = await dbConnection.QueryAsync<YourEntity>(query, new { LastDay = lastDay });

                return newData;
            }
        }

        private async Task SendEmail(IEnumerable<YourEntity> newData)
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                client.EnableSsl = true;

                using (var message = new MailMessage(_senderEmail, _recipientEmail, _subject, _body))
                {
                    // Append the new data to the email body
                    foreach (var entity in newData)
                    {
                        message.Body += $"{Environment.NewLine}{entity.ToString()}";
                    }

                    await client.SendMailAsync(message);
                }
            }
        }

        protected virtual void OnDailyEmailEvent(DailyEmailEventArgs e)
        {
            DailyEmailEvent?.Invoke(this, e);
        }
    }

    public class DailyEmailEventArgs : EventArgs
    {
        public IEnumerable<YourEntity> NewData { get; set; }
    }
}