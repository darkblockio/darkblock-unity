// ██████   █████  ██████  ██   ██ ██████  ██       ██████   ██████ ██   ██
// ██   ██ ██   ██ ██   ██ ██  ██  ██   ██ ██      ██    ██ ██      ██  ██
// ██   ██ ███████ ██████  █████   ██████  ██      ██    ██ ██      █████
// ██   ██ ██   ██ ██   ██ ██  ██  ██   ██ ██      ██    ██ ██      ██  ██
// ██████  ██   ██ ██   ██ ██   ██ ██████  ███████  ██████   ██████ ██   ██

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using Newtonsoft.Json;
using System.Text;
// import textmesh pro
using TMPro;
#if UNITY_WEBGL
public class DarkblockExample : MonoBehaviour
{
    [SerializeField]
    string tokenId = "token_id";

    [SerializeField]
    string contractAddress = "contract_address";

    [SerializeField]
    string chain = "Solana";
    string responseString = "";

    [SerializeField]
    string[] artId;
    string proxyUri;
    string sessionToken;
    public int step = 0;
    int epoch;
    string signature;
    // uncommnet to use the login button
    // public GameObject loginButton;

    [SerializeField]
    // hardcoded wallet address for testing
    public string walletAddress = "0x7401FCc471528620fDd6c3DE9EeA896e0cED6A83";

    [SerializeField]
    string assetName = "SampleAsset";
    public bool isExpired = true;
    public long secondsLeft = 0;

    // logging the time left for the asset to expire
    long clockTime = 0;
    long clockTick = 0;
    long timeNow = 0;

    // Loading message
    public GameObject loading;
    int loadingStep = 0;
    int loadingStepCount = 0;

    ///////////////////////////////////////////
    // classes for the response from the API //
    [System.Serializable]
    public class Darkblock
    {
        [JsonProperty(
            "status",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string Status { get; set; }

        [JsonProperty(
            "darkblock",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public DarkblockClass DarkblockDarkblock { get; set; }

        [JsonProperty(
            "dbstack",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public List<DarkblockClass> Dbstack { get; set; }

        [JsonProperty(
            "expiration",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public Expiration Expiration { get; set; }
    }

    [System.Serializable]
    public class Expiration
    {
        [JsonProperty(
            "in_seconds",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public long InSeconds { get; set; }

        [JsonProperty(
            "start",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public long Start { get; set; }

        [JsonProperty(
            "end",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public long End { get; set; }
    }

    [System.Serializable]
    public class DarkblockClass
    {
        [JsonProperty(
            "id",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string Id { get; set; }

        [JsonProperty(
            "tags",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public List<Tag> Tags { get; set; }

        [JsonProperty(
            "data",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public Data Data { get; set; }

        [JsonProperty("block")]
        public Block Block { get; set; }

        [JsonProperty(
            "owner",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public Owner Owner { get; set; }

        [JsonProperty(
            "signature",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string Signature { get; set; }
    }

    [System.Serializable]
    public class Block
    {
        [JsonProperty(
            "height",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public long? Height { get; set; }

        [JsonProperty(
            "timestamp",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public long? Timestamp { get; set; }

        [JsonProperty(
            "id",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string Id { get; set; }
    }

    [System.Serializable]
    public class Data
    {
        [JsonProperty(
            "size",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public long? Size { get; set; }

        [JsonProperty(
            "type",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string Type { get; set; }
    }

    [System.Serializable]
    public class Owner
    {
        [JsonProperty(
            "address",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string Address { get; set; }
    }

    [System.Serializable]
    public class Tag
    {
        [JsonProperty(
            "name",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string Name { get; set; }

        [JsonProperty(
            "value",
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore
        )]
        public string Value { get; set; }
    }

    [HideInInspector]
    public Darkblock res = new Darkblock();

    void Start()
    {
        walletAddress = walletAddress.ToLower();
        timeNow = DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    void Update()
    {
        // if the timeLeft is greater than 0, then we can display the time left every second
        if (isExpired == false)
        {
            clockTime = secondsLeft - ((timeNow - DateTimeOffset.Now.ToUnixTimeSeconds()) * -1);
            if (clockTime != clockTick)
            {
                Debug.Log(clockTime + " seconds left");
            }
            clockTick = clockTime;
        }
    }

    ////////////////////////////////
    // run this function to start //
    public void onClick()
    {
        OnSignMessage();
        // add one to the loading steps
        loadingStep++;
        // instantiate the loading screen
        // check if the loading screen is already instantiated, if not, instantiate it, if it is change the text to "Loading..."
        if (GameObject.FindGameObjectWithTag("Loading") == null)
        {
            GameObject newLoading = Instantiate(loading, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            newLoading.transform.SetParent (GameObject.FindGameObjectWithTag("Canvas").transform, false);
        }
        else
        {
            GameObject.FindWithTag("Loading").GetComponentInChildren<TextMeshProUGUI>().text = "Loading... ◙";
        }
        
    }

    ////////////////////////////////////////////////////////////////
    // sign the message to generate the session token for the api //
    async public void OnSignMessage()
    {
        System.DateTime epochStart = new System.DateTime(
            1970,
            1,
            1,
            0,
            0,
            0,
            System.DateTimeKind.Utc
        );
        epoch = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        // uncomment and change component name to an object containing the wallet address
        // walletAddress = loginButton.GetComponent<WebLogin>().account;
        // walletAddress = walletAddress.ToLower();

        try
        {
            string data = epoch + walletAddress;
            string message =
                "You are unlocking content via the Darkblock Protocol.\n\nPlease sign to authenticate.\n\nThis request will not trigger a blockchain transaction or cost any fee.\n\nAuthentication Token: "
                + data;
            string response = await Web3GL.Sign(message);
            print(response);
            signature = response;
            sessionToken = epoch + "_" + signature;
            Debug.Log(sessionToken);
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
        step++;
        Debug.Log(
            $"https://api.darkblock.io/v1/darkblock/info?nft_id={contractAddress}%3A{tokenId}&nft_platform={chain}"
        );
        StartCoroutine(
            GetMetadata(
                $"https://api.darkblock.io/v1/darkblock/info?nft_id={contractAddress}%3A{tokenId}&nft_platform={chain}"
            )
        );
    }

    //////////////////////////////////////////////////////////////////////
    // Ienumerator to send a get request to darkblock api info endpoint //
    IEnumerator GetMetadata(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    responseString = webRequest.downloadHandler.text;
                    Darkblock res = JsonConvert.DeserializeObject<Darkblock>(responseString);
                    Debug.Log(res.Status);
                    //Debug.Log(res.Dbstack[0].Tags[0].Value);
                    if (res.Expiration != null)
                    {
                        isExpired = false;
                        Debug.Log("Darkblock is not expired");
                    }
                    artId = new string[res.Dbstack.Count];
                    for (int i = 0; i < res.Dbstack.Count; i++)
                    {
                        artId[i] = res.Dbstack[i].Tags[5].Value;
                    }
                    if (artId.Length > 1)
                    {
                        Debug.Log(artId.Length + " Darkblocks found");
                    }
                    else
                    {
                        Debug.Log(artId.Length + " Darkblock found");
                    }
                    for (int i = 0; i < artId.Length; i++)
                    {
                        Debug.Log("artId: " + artId[i]);
                    }

                    break;
            }
        }

        // next step get asset-bundles
        step++;
        StartCoroutine(GetAsset());
    }

    IEnumerator GetAsset()
    {
        /////////////////////////////////////////////////////////////////////////////////////
        // uncomment and change artId[0] to artId[i] to get all asset-bundles from the NFT //
        // artId[0] is the lates asset-bundle added to the NFT
        // for (int i = 0; i < artId.Length; i++)
        // {
        proxyUri =
            $"https://gateway.darkblock.io/proxy?artid={artId[0]}&session_token={sessionToken}&token_id={tokenId}&contract={contractAddress}&platform={chain}&owner={walletAddress}";
        Debug.Log(proxyUri);
        using (WWW web = new WWW(proxyUri))
        {
            yield return web;
            AssetBundle remoteAssetBundle = web.assetBundle;
            loadingStepCount++;
            if (remoteAssetBundle == null)
            {
                // get the loading screen and change the textmesh pro input feild to "Failed to download AssetBundle!"
                GameObject.FindWithTag("Loading").GetComponentInChildren<TextMeshProUGUI>().text =
                    "Failed to download AssetBundle!";

                // wait 5 seconds and then destroy the loading screen
                yield return new WaitForSeconds(3);
                // compare the loadingStep and loadingStepCount to make sure the loading screen is destroyed only if they are equal
                if (loadingStep == loadingStepCount)
                {
                yield return new WaitForSeconds(2);
                Destroy(GameObject.FindWithTag("Loading"));
                }
                
                Debug.LogError("Failed to download AssetBundle!");
                yield break;
            }
            // instantiate the asset bundle and add it to the scene, wait 5 seconds and then destroy it
            Instantiate(remoteAssetBundle.LoadAsset(assetName));
            remoteAssetBundle.Unload(false);
            // after asset-bundle is loaded into the scene destroy the gameobject with tag "Loading"
            Destroy(GameObject.FindWithTag("Loading"));
            // if the darkblock is rented, the expiration time is set to destroy the asset after the rental time is over
            if (!isExpired)
            {
                yield return new WaitForSeconds(secondsLeft);
                Debug.Log("destroying");
                Destroy(GameObject.Find("Cube(Clone)"));
            }
        }
        // }
    }
}

#endif

//              ,,,      @@     ,,,,,,,,,     @@% ,,  ,,
//              .// //& @&    ,,,,,,,,,,,,,,  //  &/  //,
//              @@( %%  %%   ,,//*********,,  #%% #%% %&
//              %%  %%  %%  ,***************. &%* *%%  %%
//              %%%  %%% %#(**@@@@*****@@@@***#%/%%%  %%%
//              (./*****/**%*****###&@&******%*********
//                         /*********&*******/
//                        ./*****************/
//                        /*******************/
//                        /////////////////////
//                        /////////////////////
//                      ,**********////********,
//                     ////****************,*/////
//                   ///////////////////////////////,
//                   %****/////////////////////***%
//                   /*#/**********************/#*/
//                   /***/*/***************,/ /***/
//                   /****    .***(...***/    /***/
//                   /****     ***/   ***/    /***/
//                   /****     ***/   ***/    /***/
//      /****        /****     ***/   ***/    /***/       /******@
//    /*********     /***/     ***/   ////    ****/     /****//////
//   /////   //////////*/      ////   ////     ////////////    ////
// /////,     .////////.       ////   ////      .////////.     ////
//   */                        ////   ////                    ////
//                             ./*(   /*.,                   ./(.
//                             .  .   .  /
//                             .  .   .  .
//                             .  .   .  .
//                             .  .   .  .
//                             . ..   .. .
//                            ///(// //(///
