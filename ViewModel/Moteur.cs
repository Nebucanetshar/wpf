using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using WPF_MOVE;
using WPF_PROJ;

namespace WPF_ENGINE;

public class Moteur : Mouvement
{
    public double _wu;
    public double _wv;
    private bool _isAnimating;
   

    public Moteur() { }
    public Moteur(double wu, double wv) : base(wu, wv)
    {
        _wu = wu;
        _wv = wv;
    }
   
    private void OnRendering(object sender, EventArgs e)
    {
        while (_wu <6.28 && _wv<6.28)
        {
            _wu += 0.01;
            _wv += 0.01;

            Orbite = new Projection
            {
                Position = new Vector3D(0, 1, 0),
                Longitude = new Vector3D(0, -1, _wv),
                Latitude = new Vector3D(_wu, -1, 0)
            };
        }
    }

    /// <summary>
    /// animation du produit vectoriel selon la variation de phi et theta 
    /// pour avoir une distance de deplacement sphérique (orbital) U^V = Wu * Wv
    /// </summary>
    public void OrbitalAnimation(bool args)
    {
        Trace.TraceInformation("Trace Animation");

        try
        {
            if (args)
                CompositionTarget.Rendering += OnRendering;
            else
                CompositionTarget.Rendering -= OnRendering;

            _isAnimating = args;
        }
        catch (Exception ex)
        {
            Trace.TraceInformation($" ERREUR MOTEUR : {ex.Message}");
        }
        
    }
}
