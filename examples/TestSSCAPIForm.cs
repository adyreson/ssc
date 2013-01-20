﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using namespace SSC;

namespace TestApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnVersion_Click(object sender, EventArgs e)
        {
            CS_SSC_API.SSC sscobj = new CS_SSC_API.SSC();
            txtData.Clear();
            txtData.AppendText("ssc version = " + sscobj.Version() + "\n");
            txtData.AppendText("ssc build info = " + sscobj.BuildInfo() + "\n");
        }

        private void btnModuleList_Click(object sender, EventArgs e)
        {
            CS_SSC_API.SSC sscobj = new CS_SSC_API.SSC();
            txtData.Clear();
            int index = 0;
            while( sscobj.ModuleInfo( index ) )
            {
                String module_name = sscobj.ModuleName();
                String description = sscobj.ModuleDescription();
                int version = sscobj.Module_Version();
                txtData.AppendText("Module: " + module_name + ", version: " + version + "\n");
                txtData.AppendText("    " + description + "\n");
                index++;
            }        
         }

        private void btnPVWatts_Click(object sender, EventArgs e)
        {
            CS_SSC_API.SSC sscobj = new CS_SSC_API.SSC("pvwattsv1");
            txtData.Clear();
            sscobj.Set_String("file_name", "AZ Phoenix.tm2" );
            sscobj.SetNumber("system_size", 1.0f );
            sscobj.SetNumber("derate", 0.77f );
            sscobj.SetNumber("track_mode", 0 );
            sscobj.SetNumber("tilt", 20 );
            sscobj.SetNumber("azimuth", 180 );
            if ( sscobj.Exec() )
            {
                float[] ac = sscobj.GetArray( "ac" );
                float sum = 0;
            
                for( int i=0;i<ac.Count();i++)
                {
                    sum += ac[i];
                }
                txtData.AppendText("length returned: " + ac.Count() + "\n");
                txtData.AppendText("ac total (get array): " + sum + "\n");
                txtData.AppendText("PVWatts example passed" + "\n");
            }
            else
            {
                txtData.AppendText("PVWatts example failed" + "\n");
            }
        }

        private void btnPVWattsFunc_Click(object sender, EventArgs e)
        {
            CS_SSC_API.SSC sscobj = new CS_SSC_API.SSC("pvwattsfunc");
            txtData.Clear();
            sscobj.SetNumber( "year", 1970); // general year (tiny effect in sun position)
            sscobj.SetNumber( "month", 1); // 1-12
            sscobj.SetNumber( "day", 1); //1-number of days in month
            sscobj.SetNumber( "hour", 9); // 0-23
            sscobj.SetNumber( "minute", 30); // minute of the hour (typically 30 min for midpoint calculation)
            sscobj.SetNumber( "lat", 33.4f); // latitude, degrees
            sscobj.SetNumber( "lon", -112); // longitude, degrees
            sscobj.SetNumber( "tz", -7); // timezone from gmt, hours
            sscobj.SetNumber( "time_step", 1); // time step, hours

            // solar and weather data
            sscobj.SetNumber( "beam", 824); // beam (DNI) irradiance, W/m2
            sscobj.SetNumber( "diffuse", 29); // diffuse (DHI) horizontal irradiance, W/m2
            sscobj.SetNumber( "tamb", 9.4f); // ambient temp, degree C
            sscobj.SetNumber( "wspd", 2.1f); // wind speed, m/s
            sscobj.SetNumber( "snow", 0); // snow depth, cm (0 is default - when there is snow, ground reflectance is increased.  assumes panels have been cleaned off)

            // system specifications
            sscobj.SetNumber( "system_size", 4); // system DC nameplate rating (kW)
            sscobj.SetNumber( "derate", 0.77f); // derate factor
            sscobj.SetNumber( "track_mode", 0); // tracking mode 0=fixed, 1=1axis, 2=2axis
            sscobj.SetNumber( "azimuth", 180); // azimuth angle 0=north, 90=east, 180=south, 270=west
            sscobj.SetNumber( "tilt", 20); // tilt angle from horizontal 0=flat, 90=vertical


            // previous timestep values of cell temperature and POA
            sscobj.SetNumber( "tcell", 6.94f); // calculated cell temperature from previous timestep, degree C, (can default to ambient for morning or if you don't know)
            sscobj.SetNumber( "poa", 84.5f); // plane of array irradiance (W/m2) from previous time step

            if (sscobj.Exec())
            {
                float poa = sscobj.GetNumber("poa");
                float tcell= sscobj.GetNumber("tcell");
                float dc = sscobj.GetNumber("dc");
                float ac = sscobj.GetNumber("ac");
                txtData.AppendText("poa: " + poa + " W/m2\n");
                txtData.AppendText("tcell: " + tcell + " C\n");
                txtData.AppendText("dc: " + dc + " W\n");
                txtData.AppendText("ac: " + ac + " W\n");
            }

        }

        private void btnArrayTest_Click(object sender, EventArgs e)
        {
            CS_SSC_API.SSC sscobj = new CS_SSC_API.SSC();
            txtData.Clear();
            float[] arr = new float[10];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = i/10.0f;
            }
            sscobj.SetArray("TestArray", arr);

            float[] retArray = sscobj.GetArray("TestArray");

            txtData.AppendText("Testing SetArray and GetArray\n");
            for (int i = 0; i < retArray.Length; i++)
            {
                txtData.AppendText("\treturned array element: " + i + " = " + retArray[i] + "\n");
            }
        }

        private void btnTestMatrices_Click(object sender, EventArgs e)
        {
            CS_SSC_API.SSC sscobj = new CS_SSC_API.SSC();
            txtData.Clear();
            float[,] matrix = { {1,2,3}, {4,5,6}, {7,8,9} };
            sscobj.SetMatrix("TestMatrix", matrix);

            float[,] retMatrix = sscobj.GetMatrix("TestMatrix");

            txtData.AppendText("Testing SetMatrix and GetMatrix\n");
            for (int i = 0; i < retMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < retMatrix.GetLength(1); j++)
                {
                    txtData.AppendText("\treturned matrix element: (" + i + "," + j + ") = " + retMatrix[i,j] + "\n");
                }
            }
        }
    }
}
