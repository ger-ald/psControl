# psControl
Control program for korad ka3005 power supply (and copies) for use on windows XP and up.

**Screenshot of running program:**<br>
![image](https://user-images.githubusercontent.com/10982994/107150512-60fd7e00-695e-11eb-81eb-2b7f83ec60c4.png)<br>
<sup>(orange titlebar not included:wink:)</sup>

**Path to compiled bin:**<br>
/pscontrol/bin/Release/pscontrol.exe

**Logging usage:**
1. Click `More>>` to show the extended functionality
2. Set the interval or do this while it's logging<br>
<sup>Set to '0' to log as fast as update rate but note that not all csv tools handle this correctly when reading the logged csv</sup>
3. Click `Start log`, choose a filename and open<br>
<sup>The filename is `psuLog_<yyyy-MM-dd_HH.mm.ss>.csv` by default</sup>

**Scripting usage:**
1. Click `More>>` to show the extended functionality
2. Click the button with the file icon (![BrowseIcon](https://user-images.githubusercontent.com/10982994/107150805-fd745000-695f-11eb-96b5-9efbd644f659.png))<br>
3. A file browse window will pop-up where you can pick the script file to open<br>
4. Click the play button to start running the script<br>
<sup>See the example script `psuScript.txt` to see available instructions/commands</sup>
5. Click the play button again (now a pause icon) to pause the running script or the button with the stop icon to stop the script. Clicking play after stop or script finish will reload the script from disk.<br>

**Known issues:**<br>
-After changing comport numbers in device explorer, psControl will not be able to connect to that port until after a reboot.<br>
-you can still adjust the output when running a script. (wich is dangerous when using v++ or i++ in the script)<br>
-script syntax errors are not displayed

**Rundown of changes:** (summarised)<br>
v1.3.0.0:<br>
-added retries on the connect check for device<br>
-BetterSerialPort.cs -> pHandle was incorrectly checked for invalid<br>
-improved output enabled visibility<br>
-added scripting<br>

v1.2.0.0:<br>
-add logging<br>
-add html file to view logged csv*<br>
-code cleanup<br>
-moved higher level code for psu from form1 to it's own class<br>

<sup>* Open in browser and select file bottom left. This can display the csv with "as fast as update rate" correctly.<br>
 Note that HighCharts is used for drawing the graph which has a different lisence than this tool.</sup>

v1.1.2.0:<br>
-added load/save feature<br>
-rewrote setpoint storage part (exe side) to work towards a nice contained psu class
