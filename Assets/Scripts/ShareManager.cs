using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.NativePlugins;

public enum ShareOn { Facebook, Whatsapp}

public class ShareManager : MonoBehaviour {


    static public ShareManager S;

        
	void Awake()
    {
        if (S != null)
            Destroy(gameObject);
        S = this;
    }
    
    public void ShareOnFacebook()
    {
        FBShareComposer fbShare = new FBShareComposer();
        
       
        fbShare.Text = "I Rule! Just set a new record on RunJoeRun";//m_shareMessage;

        // Add below line if you want to share URL
        //_shareSheet.URL = m_shareURL;

        // Add below line if you want to share a screenshot
        fbShare.AttachScreenShot();

        // Add below line if you want to share an image from a specified path.
        //_shareSheet.AttachImageAtPath(IMAGE_PATH);

        // Show composer
        NPBinding.UI.SetPopoverPointAtLastTouchPosition(); // To show popover at last touch point on iOS. On Android, its ignored.
        if (fbShare.IsReadyToShowView)
            NPBinding.Sharing.ShowView(fbShare, FinishedSharing);
    }

    public void ShareOnWhatsapp()
    {
        WhatsAppShareComposer wShare = new WhatsAppShareComposer();
        

        wShare.Text = "I Rule! Just set a new record on RunnerJoe";//m_shareMessage;

       

        // Add below line if you want to share a screenshot
        //wShare.AttachScreenShot();

        

        // Show composer
        NPBinding.UI.SetPopoverPointAtLastTouchPosition(); // To show popover at last touch point on iOS. On Android, its ignored.
        if (wShare.IsReadyToShowView)
            NPBinding.Sharing.ShowView(wShare, FinishedSharing);
    }

    public void ShareGeneral()
    {
        // Create share sheet
        ShareSheet _shareSheet = new ShareSheet();
        _shareSheet.Text = "I Rule! Just set a new record on Run Joe Run";

        // Set this list if you want to exclude any service/application type. Else, ignore.
        //_shareSheet.ExcludedShareOptions = m_excludedOptions;

        // Attaching screenshot here
        _shareSheet.AttachScreenShot();

        // Show composer
        NPBinding.UI.SetPopoverPointAtLastTouchPosition();
        NPBinding.Sharing.ShowView(_shareSheet, FinishedSharing);
    }

    void FinishedSharing(eShareResult _result)
    {
        Debug.Log("Finished sharing");
        Debug.Log("Share Result = " + _result);
    }
    
    
}
