# WindowsCommands
![Image 1](Screenshots/Screen1.png)

This project is a command-line application built in C#. It provides a variety of commands for retrieving system information, managing network interfaces, and more.
This project was inspired by the [PS-Commands](https://github.com/Lifailon/PS-Commands) project on GitHub. I wanted to create a similar tool in C# that would provide similar functionality. This project is still under development.

## Features
The application currently supports the following commands:

- Get files from a specified path
- Get event log
- Get network interface stats
- Get process performance
- Get user session
- Get web certificate info
- Start TCP server
- Start UDP server
- Ping network
- Get system info
- Get memory info
- Get memory slots info
- Get CPU info
- Get driver info
- Get disk info
- Get IO info
- Get ARP table
- Get network adapter info
- Get network configuration
- Monitor network utilization
- Get temperature ***
- Get video card info
- Get Windows update info
- Get Battery information
- Clean old temporary files
- Download images from a specified URL **
- Scan and list all network devices
- Run all cleanup tasks
- Change access rights for a specified file or directory
- Analyze WiFi networks
- Download YouTube videos and optionally convert to MP3 ****
- Port Scanner
- Compute SHA-256 hash of a text file

** The `Download images from a specified URL` command saves the images to the `My Pictures` folder.
*** The `Get temperature` command may not work on many systems due to the hardware support. 
**** The Download YouTube videos and optionally convert to MP3 command works correctly only with a specific video quality (1: 360p Mp4). Currently, this command is not working.

All results are logged and saved to a folder named WindowsCommands in the My Documents directory.

Many commands come with its own set of options that allow you to customize the command's behavior.

## Usage

1. Open a command prompt as administrator.
2. Navigate to the directory containing the utility.
3. Run the utility as a command-line argument. For example:

    ```
    .\WindowsCommands.exe
    ```
4. When you start the application, you'll see a title and an available commands.

## Running with Docker

You can also run this application using Docker. To do so, follow these steps:

1. Ensure you have Docker and Docker Compose installed on your system.
2. Navigate to the directory containing the `docker-compose.yml` file.
3. Build and run the Docker container using Docker Compose:

    ```sh
    docker-compose run --rm --service-ports windowscommands
    ```

Please note that most commands will not work when running from within Docker due to the limitations of accessing system resources from a container. For example, the `port-scanner` command will work, but commands that require direct access to system hardware or the Windows operating system will not function correctly.

## Future Development

This project is still under development. I plan to add more commands in the future to further enhance the functionality of the application. Stay tuned for updates!

## Contributions

We welcome contributions from the community. If you have a command you'd like to add, feel free to open a pull request.

## Author

Bohdan Harabadzhyu

## License

[MIT](https://choosealicense.com/licenses/mit/)
