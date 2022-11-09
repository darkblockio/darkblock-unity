
<a name="DarkblockUnity"></a>



<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://www.darkblock.io">
    <img src="https://www.darkblock.io/wp-content/uploads/2022/05/White@2x.png" alt="Logo">
  </a>

<h3 align="center">Darkblock Unity</h3>

  <p align="center">
    A script to integrate Darkblock support in web3 unity games
    <br />
    <a href="https://docs.darkblock.io"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <!-- <a href="https://github.com/raggi-eth/DarkblockUnity">View Demo</a> -->
    ·
    <a href="https://github.com/raggi-eth/DarkblockUnity/issues">Report Bug</a>
    ·
    <a href="https://github.com/raggi-eth/DarkblockUnity/issues">Request Feature</a>
  </p>
</div>



<!-- ABOUT THE PROJECT -->
## About The Project

A simple script to showcase how to integrate Darkblock support in web3 unity games. The script is set up to support the <a href="https://github.com/ChainSafe/web3.unity"> Chainsafe web3 unity package</a>, but can be easily adapted to work with other web3 packages.

The Darkblock protocol is a decentralized solution for unlockable digital assets. NFT creators can add Darkblock unlockable content to their NFTs, transforming the NFTs into keys that unlock the content. 

This enables web3 game developers to sell NFTs that unlock certain aspects of the game, such as new levels, characters, weapons, etc.

By purchasing the NFTs players can unlock unity AssetBundles stored in Darkblocks and import them into the game at runtime.



<!-- GETTING STARTED -->
## Getting Started

After minting the NFTs and creating the AssetBundles you are ready to create the Darkblocks. You can either do this by using the <a href="https://docs.darkblock.io/openapi/core/tag/Darkblock-API/"> Darkblock API </a> or the <a href="https://app.darkblock.io">Darkblock Web App.</a>

Once you have created the Darkblocks you can use the script to pull the AssetBundles from the Darkblocks and import them into the game. simply add the contract address and TokenId of the NFT to the script and you are ready to go.

the script will check if the NFT is owned by the user and if it is it will pull the AssetBundles from the Darkblock and import it into the game.

this action requires the user to sign a message with their wallet, so the script will also prompt the user to sign the message.

this is a simple example of how to integrate Darkblock support in web3 unity games. the script can be easily adapted to work with other web3 packages.



### Prerequisites

Unity 2019.2 or higher </br>
<a href="https://docs.unity3d.com/Packages/com.unity.assetbundlebrowser@1.7/manual/index.html"> Asset Bundle Browser </a> </br>
<a href="https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@3.0/manual/index.html">Newtonsoft Json</a>



### Installation

1. Clone the repo
   ```sh
   git clone https://github.com/raggi-eth/DarkblockUnity.git

2. Import the Asset Bundle Browser and Newtonsoft Json packages



<!-- USAGE EXAMPLES -->
## Usage

1. Add the DarkblockUnity script to an empty game object in your scene

2. Add the contract address and token-Id of the NFT to the script

3. Trigger the onClick() function of the script with a button or other in-game event

4. The script will check if the NFT is owned by the user and if it is it will pull the AssetBundles from the Darkblock and import it into the game.



<!-- ROADMAP -->
## Roadmap

- [x] Add support for EVM chains
- [x] Add support for Darkblocks with multiple AssetBundles
- [ ] Add Solana support
- [ ] Create a Darkblock unity package for easy integration and demo scene


See the [open issues](https://github.com/raggi-eth/DarkblockUnity/issues) for a full list of proposed features (and known issues).



<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request



<!-- LICENSE -->
## License

Distributed under the DWTFYW License. 



<!-- CONTACT -->
## Contact

Darkblockio - [@darkblockio](https://twitter.com/darkblockio) - info@darkblock.io

Project Link: [https://github.com/raggi-eth/DarkblockUnity](https://github.com/raggi-eth/DarkblockUnity)









