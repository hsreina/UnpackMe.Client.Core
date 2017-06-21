using System;
using System.IO;
using System.Linq;
using UnpackMe.SDK.Core;
using UnpackMe.SDK.Core.Models;

namespace UnpackMe.Client.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            using (UnpackMeClient unpackMeClient = new UnpackMeClient("http://unpackmeurl"))
            {
                unpackMeClient.Authenticate("login", "password");

                // Retrieve the list of available commands for your login/password
                CommandModel[] commands = unpackMeClient.GetAvailableCommands();
                CommandModel decryptCommand = commands.SingleOrDefault(x => x.CommandTitle == "Pangya TH *.iff decrypt");

                // Open the file to unpack
                FileStream fileStream = new FileStream(
                    @"C:\UnpackMe\pangya.iff",
                    FileMode.Open
                );

                // Create an unpack task with the file
                string taskId = unpackMeClient.CreateTaskFromCommandId(decryptCommand.CommandId, fileStream);

                // Check for unpack status
                TaskModel task;
                string taskStatus;
                do
                {
                    task = unpackMeClient.GetTaskById(taskId);
                    taskStatus = task.TaskStatus;

                    System.Threading.Thread.Sleep(1000);

                    Console.WriteLine(taskStatus);

                } while (taskStatus != "completed");

                // When unpacked file is ready, Save the result into a file
                unpackMeClient.SaveTaskFileTo(
                    taskId,
                    @"C:\UnpackMe\pangya.iff.dec"
                );

                Console.WriteLine("should be unpacked now o_O");
            }
        }
    }
}