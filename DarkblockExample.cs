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
    public GameObject loginButton;
    [SerializeField]
    // hardcoded wallet address for testing 
    public string walletAddress = "0x7401FCc471528620fDd6c3DE9EeA896e0cED6A83";
    [SerializeField]
    string assetName = "SampleAsset";
    
    void Start()
    {
        walletAddress = walletAddress.ToLower();
    }


    ////////////////////////////////
    // run this function to start //
    public async void onClick () {
        OnSignMessage();

    }


    ///////////////////////////////////////////
    // classes for the response from the API //
    [System.Serializable]
    public class Darkblock
    {
        [JsonProperty("status", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty("darkblock", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public DarkblockClass DarkblockDarkblock { get; set; }

        [JsonProperty("dbstack", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<DarkblockClass> Dbstack { get; set; }
    }
    [System.Serializable]
    public class DarkblockClass
    {
        [JsonProperty("id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("tags", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<Tag> Tags { get; set; }

        [JsonProperty("data", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Data Data { get; set; }

        [JsonProperty("block", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Block Block { get; set; }

        [JsonProperty("owner", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Owner Owner { get; set; }

        [JsonProperty("signature", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Signature { get; set; }
    }
    [System.Serializable]
    public class Block
    {
        [JsonProperty("height", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Height { get; set; }

        [JsonProperty("timestamp", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Timestamp { get; set; }

        [JsonProperty("id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
    }
    [System.Serializable]
    public class Data
    {
        [JsonProperty("size", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Size { get; set; }

        [JsonProperty("type", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
    }
    [System.Serializable]
    public class Owner
    {
        [JsonProperty("address", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
    }
    [System.Serializable]
    public class Tag
    {
        [JsonProperty("name", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("value", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }
    }
    [HideInInspector]
    public Darkblock res = new Darkblock();


    //////////////////////////////////////////////////////////////////////
    // Ienumerator to send a get request to darkblock api info endpoint //
     IEnumerator GetArtId(string uri)
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
                    artId = new string[res.Dbstack.Count];
                    for(int i = 0; i < res.Dbstack.Count; i++)
                        {
                        artId[i] = res.Dbstack[i].Tags[5].Value;
                        }
                    if(artId.Length > 1){
                    Debug.Log(artId.Length + " Darkblocks found");
                    }
                    else{
                    Debug.Log(artId.Length + " Darkblock found");
                    }
                    for (int i = 0; i < artId.Length; i++)
                    {
                        Debug.Log("artId: "+artId[i]);
                    }    
                    break;       
            }
        }

    // next step get asset-bundles
    step++;
    StartCoroutine(GetAsset());
    }

    IEnumerator GetAsset(){
    /////////////////////////////////////////////////////////////////////////////////////    
    // uncomment and change artId[0] to artId[i] to get all asset-bundles from the NFT //
    // artId[0] is the lates asset-bundle added to the NFT
    // for (int i = 0; i < artId.Length; i++)
    // {
        proxyUri = $"https://gateway.darkblock.io/proxy?artid={artId[0]}&session_token={sessionToken}&token_id={tokenId}&contract={contractAddress}&platform={chain}&owner={walletAddress}";
        Debug.Log(proxyUri);
        using (WWW web = new WWW(proxyUri))
        {
            yield return web;
            AssetBundle remoteAssetBundle = web.assetBundle;
            if (remoteAssetBundle == null) {
                Debug.LogError("Failed to download AssetBundle!");
                yield break;
            }
            Instantiate(remoteAssetBundle.LoadAsset(assetName));
            remoteAssetBundle.Unload(false);
        }
    // }
    }


////////////////////////////////////////////////////////////////
// sign the message to generate the session token for the api //
async public void OnSignMessage()       
    {   
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        epoch = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        // uncomment and change component name to an object containing the wallet address
        // walletAddress = loginButton.GetComponent<WebLogin>().account;
        // walletAddress = walletAddress.ToLower();

        try {
            string data = epoch + walletAddress;
            string message = "You are unlocking content via the Darkblock Protocol.\n\nPlease sign to authenticate.\n\nThis request will not trigger a blockchain transaction or cost any fee.\n\nAuthentication Token: " + data;
            string response = await Web3GL.Sign(message);
            print(response);
            signature = response;
            sessionToken = epoch + "_" + signature;
            Debug.Log(sessionToken);

        } catch (Exception e) {
            Debug.LogException(e, this);
        }
        step++;
        StartCoroutine(GetArtId($"https://api.darkblock.io/v1/darkblock/info?nft_id={contractAddress}:{tokenId}&nft_platform={chain}"));
    }
}

#endif