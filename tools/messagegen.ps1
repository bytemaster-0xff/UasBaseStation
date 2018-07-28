$path = (Split-Path $MyInvocation.MyCommand.Path);
$path
&"$path\mavlinkgen.exe" --output="$path\..\src\LagoVista.MavLink\GeneratedMessage.cs" "$path\..\messages\ardupilotmega.xml" 