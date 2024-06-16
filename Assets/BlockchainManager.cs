using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using UnityEngine.UI;
using GamePolygon;

public class BlockchainManager : MonoBehaviour
{
    public GameObject panelMenu;
    public Button claimNFT;
    public Text claimNFTText;
    public Button x2Gold;
    public Text x2GoldText;
    public Button loadLevel;
    public Text loadLevelText;
    public Button playBtn;
    public Button characterBtn;

    private void Start()
    {
        panelMenu.SetActive(false);
        claimNFT.gameObject.SetActive(false);
        claimNFTText.text = "Claim NFT";
        x2Gold.gameObject.SetActive(false);
        x2GoldText.text = "+1000 Gold";
        loadLevel.gameObject.SetActive(false);
        loadLevelText.text = "Load Level";

    }

    public void OnWalletConnected() {
        GetNFTBalance();
    }

    public async void GetNFTBalance() {
        string address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        Contract contract = ThirdwebManager.Instance.SDK.GetContract("0x9C2c96AD0B1e7FFCe83fD5E7Dd1A8095AbaEf9e2");
        List<NFT> nftList = await contract.ERC721.GetOwned(address);
        if (nftList.Count > 0)
        {
            panelMenu.SetActive(true);
            claimNFT.gameObject.SetActive(false);
            claimNFTText.text = "Claim NFT";
            claimNFT.interactable = true;

            x2Gold.gameObject.SetActive(true);
            x2Gold.interactable = true;
            x2GoldText.text = "+1000 Gold";

            loadLevel.gameObject.SetActive(true);
            loadLevel.interactable = true;
            loadLevelText.text = "Load Level";
        }
        else
        {
            claimNFT.gameObject.SetActive(true);
            claimNFT.interactable = true;
            claimNFTText.text = "Claim NFT";
        }
    }

    public async void ClaimTokenGateNFT() {
        claimNFT.interactable = false;
        claimNFTText.text = "Claiming...";
        Contract contract = ThirdwebManager.Instance.SDK.GetContract("0x9C2c96AD0B1e7FFCe83fD5E7Dd1A8095AbaEf9e2");
        var data = await contract.ERC721.Claim(1);
        GetNFTBalance();
    }

    public int ConvertFloatToInt(float input)
    {
        if (input < 2f)
        {
            return 1;
        }
        else if (input > 53f)
        {
            return 53;
        }
        else
        {
            return Mathf.RoundToInt(input);
        }
    }

    private int ConvertLevelToInt(float valueToConvert) {
        int result1 = ConvertFloatToInt(valueToConvert);
        return result1;
    }

    public async void LoadLevelForPlayer()
    {
        loadLevel.interactable = false;
        x2Gold.interactable = false;
        playBtn.interactable = false;
        characterBtn.interactable = false;
        loadLevelText.text = "Loading...";
        string address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        Contract contract = ThirdwebManager.Instance.SDK.GetContract("0xB8586A6615d67eEF3309A99331Bd3DF0A510d28D");
        var data = await contract.ERC20.BalanceOf(address);
        int blockchainLevel = ConvertLevelToInt(float.Parse(data.displayValue));
        int CurrentLevel = PlayerPrefs.GetInt("Level", 1);
        if (blockchainLevel > 1)
        {
            if (blockchainLevel > CurrentLevel)
            {
                PlayerPrefs.SetInt("Level", blockchainLevel);
            }
        }
        loadLevelText.text = PlayerPrefs.GetInt("Level", 1).ToString();
        x2Gold.interactable = true;
        playBtn.interactable = true;
        characterBtn.interactable = true;
    }

    public async void ClaimERC20Token() {
        loadLevel.interactable = false;
        x2Gold.interactable = false;
        playBtn.interactable = false;
        characterBtn.interactable = false;
        x2GoldText.text = "Claiming...";
        Contract contract = ThirdwebManager.Instance.SDK.GetContract("0x5710636e4b2C804eDA5f8E6165d4C1Cb51C75B03");
        var data = await contract.ERC20.Claim("1");
        Debug.Log("Tokens were claimed");
        x2GoldText.text = "+1000 Gold";
        CoinManager.Instance.Coins += 1000;
        x2Gold.interactable = true;
        playBtn.interactable = true;
        characterBtn.interactable = true;
        loadLevel.interactable = true;
    }
}
