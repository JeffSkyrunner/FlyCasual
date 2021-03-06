using Editions;
using Ship;
using SquadBuilderNS;
using UnityEngine;
using UnityEngine.UI;
using Upgrade;

public class SmallCardArt : MonoBehaviour {

    private string ImageUrl;
    private IImageHolder ImageSource;
    private const string TEXTURENAME = "SMALLCARD_";
    private string textureCacheKey = "";

    public void Initialize(IImageHolder imageSource)
    {
        ImageSource = imageSource;
        ImageUrl = ImageSource.ImageUrl;

        this.gameObject.SetActive(false);
        textureCacheKey = TEXTURENAME + ImageUrl;
        if (ImageUrl != null) LoadTooltipImage(gameObject, ImageUrl);
    }

    private void LoadTooltipImage(GameObject thisGameObject, string url)
    {
        if (!TextureCache.Cache.ContainsKey(textureCacheKey))
        {
            Global.Instance.StartCoroutine(ImageManager.GetTexture((texture) =>
            {
                if (thisGameObject != null)
                {
                    if (texture != null)
                    {
                        SetObjectSprite(texture, ImageSource, thisGameObject, false);
                    }
                    else
                    {
                        ShowTextVersionOfCard();
                    }
                }
            }, url)); 
        }
        else
        {
            if (thisGameObject != null)
            {
                SetObjectSprite(TextureCache.Cache[textureCacheKey], ImageSource, thisGameObject, true);
            }
        }
    }

    private void SetObjectSprite(Texture2D newTexture, object imageSource, GameObject targetObject, bool textureIsScaled)
    {
        Rect imageRect = new Rect();
        if (imageSource is GenericShip)
        {
            if (Edition.Current is SecondEdition)
            {
                if (!textureIsScaled) TextureScale.Bilinear(newTexture, 503, 700);
                imageRect = new Rect(0, 0, 503, 205);
            }
        }
        else if (imageSource is GenericUpgrade)
        {
            if (Edition.Current is SecondEdition)
            {
                if (!textureIsScaled) TextureScale.Bilinear(newTexture, 700, 503);

                if (!(imageSource as GenericUpgrade).HasType(UpgradeType.Configuration))
                {
                    imageRect = new Rect(281, 0, 394, 202);
                }
                else
                {
                    imageRect = new Rect(25, 0, 394, 202);
                }
            }
            else
            {
                if (!textureIsScaled) TextureScale.Bilinear(newTexture, 194, 300);
                imageRect = new Rect(0, 0, 194, 103);
            }
        }
        if (!TextureCache.Cache.ContainsKey(textureCacheKey)) TextureCache.Cache.Add(textureCacheKey, newTexture);
        Sprite newSprite = Sprite.Create(
            newTexture,
            new Rect(
                imageRect.x,
                newTexture.height - imageRect.height - imageRect.y,
                imageRect.width,
                imageRect.height),
            Vector2.zero
        );

        targetObject.transform.GetComponent<Image>().sprite = newSprite;
        targetObject.GetComponent<RectTransform>().sizeDelta = new Vector2(188, 188 / imageRect.width * imageRect.height);

        this.gameObject.SetActive(true);
    }

    private void ShowTextVersionOfCard()
    {
        try
        {
            this.gameObject.SetActive(true);
        }
        catch { }
    }
}
