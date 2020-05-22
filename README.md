# psControl
Control program for korad ka3005 power supply (and copies) for use on windows XP and up.

**Screenshot of running program:**<br>
![image](https://user-images.githubusercontent.com/10982994/82653163-121c8680-9c1f-11ea-9592-e0f91d8305c9.png)<br>
<sup>(orange titlebar not included:wink:)</sup>

**Path to compiled bin:**<br>
/pscontrol/bin/Release/pscontrol.exe

**Logging usage:**
1. Click `More>>` to show the extended functionality
2. Set the interval or do this while it's logging<br>
<sup>Set to '0' to log as fast as update rate but note that not all csv tools handle this correctly when reading the logged csv</sup>
3. Click `Start log`, choose a filename and open<br>
<sup>The filename is `psuLog_<yyyy-MM-dd_HH.mm.ss>.csv` by default</sup>

**Rundown of changes:** (summarised)<br>
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
