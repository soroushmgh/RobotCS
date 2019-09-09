using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RobotCS.Enums;

namespace RobotCS
{
    public class MyRobot
    {

        private const int MaxRowIndex = 4;
        private const int MaxColumnIndex = 4;
        private const string WestStr = "WEST", NorthStr = "NORTH", EastStr = "EAST", SouthStr = "SOUTH";
        private const string MoveStr = "MOVE", RightStr = "RIGHT", LeftStr = "LEFT", ReportStr = "REPORT", PlaceStr = "PLACE";

        private bool _isFirstRequest = true;
        private string _face = string.Empty;
        private string CommandType { get; set; } = string.Empty;

        private int _rowIndex;
        private int RowIndex
        {
            get => _rowIndex;
            set => _rowIndex = value >= 0 && value <= MaxRowIndex
                ? value
                : throw new Exception("The entered row index is out of range. Min and Max allowed values for row are: 0 and " + MaxRowIndex);
        }

        private int _columnIndex;
        private int ColumnIndex
        {
            get => _columnIndex;
            set => _columnIndex = value >= 0 && value <= MaxColumnIndex
                ? value
                : throw new Exception("The entered row index is out of range. Min and Max allowed values for column are: 0 and " + MaxColumnIndex);
        }

        /// <summary>
        /// To create the object and print the instruction
        /// </summary>
        public MyRobot()
        {
            PrintOnScreen("                                      Welcome to my Robot app!" +
                          "\n******************************************** INSTRUCTION *******************************************" +
                          "\n****************************Please type \"EXIT\" to close the application*****************************" +
                          "\nPLACE will put the toy robot on the table in position X,Y and facing NORTH, SOUTH, EAST or WEST." +
                          "\nThe origin (0,0) can be considered to be the SOUTH WEST most corner." +
                          "\nIt is required that the first command to the robot is a PLACE command, after that, any sequence" +
                          "\nof commands may be issued, in any order, including another PLACE command." +
                          "\n****************************************************************************************************",
                MessageTypes.Instruction);
        }

        /// <summary>
        /// To validate user's input
        /// </summary>
        /// <param name="command"></param>
        private void ValidateCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                throw new Exception("The input string can't be empty or white space");
            }

            if (_isFirstRequest && !IsValidPlaceCmd(command) && !command.Equals("EXIT"))
            {
                throw new Exception("The first input should be a valid PLACE command");
            }

            var splittedCommand = command.Split(' ');
            var commandType = splittedCommand.First();
            if (!IsValidCmd(command))
            {
                throw new Exception("Please enter a valid command");
            }

            CommandType = commandType;
        }

        /// <summary>
        /// To receive user's input
        /// </summary>
        public void ReadUserInput()
        {

            while (!CommandType.Equals("EXIT", StringComparison.CurrentCultureIgnoreCase))
            {
                PrintOnScreen(
                    "\nPlease place the robot a square table top, of dimensions 5 units x 5 units (Ex: PLACE 0,0,NORTH): ",
                    MessageTypes.Query, false);
                try
                {
                    ProcessRequest(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    PrintOnScreen(ex.Message, MessageTypes.Error);
                }
            }

        }

        /// <summary>
        /// To validate and process the user's command
        /// </summary>
        /// <param name="userInput"></param>
        public void ProcessRequest(string userInput)
        {
            userInput = userInput.Trim().ToUpper();

            ValidateCommand(userInput);

            switch (CommandType)
            {
                case ReportStr:
                    PrintOnScreen(ProcessReportReq(), MessageTypes.Result);
                    break;
                case PlaceStr:
                    ProcessPlaceReq(userInput);
                    break;
                case MoveStr:
                    ProcessMoveReq();
                    break;
                case LeftStr:
                    ProcessLeftReq();
                    break;
                case RightStr:
                    ProcessRightReq();
                    break;
            }
        }

        /// <summary>
        /// To rotate the robot 90 degrees to the right
        /// </summary>
        private void ProcessRightReq()
        {
            switch (_face)
            {
                case WestStr:
                    _face = NorthStr;
                    break;
                case NorthStr:
                    _face = EastStr;
                    break;
                case EastStr:
                    _face = SouthStr;
                    break;
                case SouthStr:
                    _face = WestStr;
                    break;
            }
        }

        /// <summary>
        /// To rotate the robot 90 degrees to the left
        /// </summary>
        private void ProcessLeftReq()
        {
            switch (_face)
            {
                case WestStr:
                    _face = SouthStr;
                    break;
                case NorthStr:
                    _face = WestStr;
                    break;
                case EastStr:
                    _face = NorthStr;
                    break;
                case SouthStr:
                    _face = EastStr;
                    break;
            }
        }

        /// <summary>
        /// To move the robot forward
        /// </summary>
        private void ProcessMoveReq()
        {
            switch (_face)
            {
                case WestStr:
                    --ColumnIndex;
                    break;
                case NorthStr:
                    ++RowIndex;
                    break;
                case EastStr:
                    ++ColumnIndex;
                    break;
                case SouthStr:
                    --RowIndex;
                    break;
            }
        }

        /// <summary>
        /// To place the robot on the table
        /// </summary>
        /// <param name="userInput"></param>
        private void ProcessPlaceReq(string userInput)
        {
            var tmpRobotLocation = userInput.Split(' ').Last().Split(',');
            ColumnIndex = Int32.Parse(tmpRobotLocation.First());
            RowIndex = Int32.Parse(tmpRobotLocation[1]);
            _face = tmpRobotLocation.Last();
            _isFirstRequest = false;
        }

        /// <summary>
        /// To generate a report of the robot's status
        /// </summary>
        /// <returns></returns>
        public string ProcessReportReq()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            return new StringBuilder().Append("Output: ").Append(ColumnIndex).Append(",").Append(RowIndex)
                .Append(",").Append(_face).ToString();
        }

        /// <summary>
        /// To validate the format of a user's command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private bool IsValidCmd(string command)
        {
            if (!IsValidPlaceCmd(command))
            {
                return Regex.IsMatch(command, @"\w*(PLACE|MOVE|LEFT|RIGHT|REPORT|EXIT)$", RegexOptions.IgnoreCase);
            }

            return true;
        }

        /// <summary>
        /// To validate PLACE command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private bool IsValidPlaceCmd(string command)
        {
            return Regex.IsMatch(command, @"^PLACE \d,\d,\w*(NORTH|SOUTH|WEST|EAST)$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// To print the output on the screen
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        /// <param name="moveNextLine"></param>
        public void PrintOnScreen(string message, MessageTypes messageType, bool moveNextLine = true)
        {
            Console.ForegroundColor = ConsoleColor.White;

            switch (messageType)
            {
                case MessageTypes.Instruction:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case MessageTypes.Result:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case MessageTypes.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case MessageTypes.Query:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }

            if (moveNextLine)
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.Write(message);
            }
        }
    }
}
