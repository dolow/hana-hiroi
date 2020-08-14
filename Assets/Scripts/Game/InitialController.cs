using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitialController : MonoBehaviour
{
    [SerializeField]
    private Text textLicense = null;
    [SerializeField]
    private Text textLicenseImages = null;
    [SerializeField]
    private Text textLicenseSounds = null;
    [SerializeField]
    private Text textLicenseFonts = null;

    [SerializeField]
    private Text textAuthorGameBy = null;
    [SerializeField]
    private Text textAuthorSmith  = null;

    [SerializeField]
    private Text textCats  = null;
    [SerializeField]
    private RawImage imageCats = null;
    [SerializeField]
    private Text textCatsZ1 = null;
    [SerializeField]
    private Text textCatsZ2 = null;
    [SerializeField]
    private Text textCatsZ3 = null;
    [SerializeField]
    private Text textCatsZ4 = null;

    private int currentFrame = 0;

    void Update()
    {
        switch (this.currentFrame)
        {
            case 60: this.ToggleText(this.textLicense); break;
            case 120: this.ToggleText(this.textLicenseImages); break;
            case 180: this.ToggleText(this.textLicenseSounds); break;
            case 240: this.ToggleText(this.textLicenseFonts); break;
            case 300: this.DisableTexts(); break;
            case 360:
                {
                    this.textAuthorGameBy.gameObject.SetActive(true);
                    this.textAuthorSmith.gameObject.SetActive(true);
                    break;
                }
            case 420:
                {
                    this.textCats.gameObject.SetActive(true);
                    this.imageCats.gameObject.SetActive(true);
                    this.textCatsZ1.gameObject.SetActive(true);
                    this.textCatsZ2.gameObject.SetActive(true);
                    break;
                }
        }

        if (this.currentFrame > 420 && this.currentFrame % 30 == 0)
        {
            this.textCatsZ1.gameObject.SetActive(!this.textCatsZ1.gameObject.activeSelf);
            this.textCatsZ2.gameObject.SetActive(!this.textCatsZ2.gameObject.activeSelf);
            this.textCatsZ3.gameObject.SetActive(!this.textCatsZ3.gameObject.activeSelf);
            this.textCatsZ4.gameObject.SetActive(!this.textCatsZ4.gameObject.activeSelf);
        }


        if (this.currentFrame == 600)
        {
            SceneManager.LoadScene("TitleScene");
        }

        this.currentFrame++;
    }

    private void ToggleText(Text target)
    {
        this.DisableTexts();

        target.gameObject.SetActive(true);
    }

    private void DisableTexts()
    {
        this.textLicense.gameObject.SetActive(false);
        this.textLicenseImages.gameObject.SetActive(false);
        this.textLicenseSounds.gameObject.SetActive(false);
        this.textLicenseFonts.gameObject.SetActive(false);

        this.textAuthorGameBy.gameObject.SetActive(false);
        this.textAuthorSmith.gameObject.SetActive(false);
    }
}
