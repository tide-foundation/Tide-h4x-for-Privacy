# Tide h4x for Privacy

The [H4X experiment](http://h4x.tide.org) is a showcase of the Tide Protocol's novel authentication and data privacy protection technology, inviting the community to learn, contribute and engage with Tide on the development of the protocol. It also challenges participants to find security flaws by placing a real bitcoin behind its protection.
The concept is quite simple. The details of 1 Bitcoin were stored in the simplest website setup: on a database record behind a web server. Anyone logging in to that website using the correct username and password will see those details. All typical defences around that set up were removed. No firewalls, no added security. It should be easy work for any hacker to crack that. The twist: Tide's unique protection mechanism was used on the data and the website authentication. Supposedly, even if one cracks the whole thing, it would be impractical to crack the authentication or extract the data. Anyone who does, the bitcoin is theirs, and we're back to the drawing board. This project code is an open-source and provides ability to recreate the entire environment locally.

### Terminology

Below are terms that are important to understand within the context of the Tide Protocol.

**Seeker** - Any organization or business seeking to engage with or collect consumer data.

**Vendor** - Any consumer-facing organization or business that collects, manages and stores consumer data. In this context, the H4X website.

**Consumer** - Any individual natural person that has a uniquely identified representation or data footprint (usually in the form of a user account or identity) in a Vendor’s database. In this context: the H4X target, owner of the bitcoin wallet.

**Smart Contract** - a computer protocol intended to digitally facilitate, verify, or enforce the negotiation or performance of a contract.

**ORK** - Orchestrated Recluder of Keys - The Tide Protocol decentralized nodes

**Key Pair** - consist of a Secret Key (SK) and Public Key (PK)

## Architecture

![alt text](https://github.com/tide-foundation/Tide-h4x-for-Privacy/blob/master/Tide%20h4x%20Architecture.png "Architecture Diagram")

## Flow Diagram

![alt text](https://github.com/tide-foundation/Tide-h4x-for-Privacy/blob/master/Tide%20h4x%20Workflow.png "Flow Diagram")

### Components

1. **Raziel.Contracts** - smartcontracts for the challenge.
1. **Raziel.Creator** - account creationg for the challenge.
1. **Raziel.Front** - challenge frontend website. 
1. **Raziel.Generator** - key creation.
1. **Raziel.Library** - Tide libraries which includes encryption. 
1. **Raziel.ORK** - decentralized ORK nodes.
1. **Raziel.Vendor** - API layer that handles database access and token authentication. 

## Installation

While the live H4X environment is staged with lots of nodes, hosted across 3 different public clouds, utilizing EOS mainnet, this guide assists you to replicate the entire environment locally, on a small scale, using EOS jungle testnet (free), with just 3 ORK nodes - so it doesn't cost you.

While all the components of the this environment are cross-platform, this manual describes how to set it up in a Windows environment. Similar steps can be followed to achieve the same on Linux.

The architecture, therefore, is slightly different and looks like this:
![alt text](https://github.com/tide-foundation/Tide-h4x-for-Privacy/blob/master/Tide%20h4x%20Local.png "Local Setup")

### Prerequisite

The following components are required to be set up ahead of the deployment:
1. [.NET Core 2.2 Build apps - SDK](https://dotnet.microsoft.com/download/dotnet-core/2.2 ".net Core 2.2 Download")
1. [Node.js - LTS](https://nodejs.org/en/download/ "node.js Download")
1. [Cleos](https://developers.eos.io/eosio-nodeos/v1.2.0/docs/cleos-overview "Cleos")
1. Clone Repository `git clone https://github.com/tide-foundation/Tide-h4x-for-Privacy`

#### Installing Cleos in Windows 10

There are two options when running EOS in a Windows environment: Docker or Linux for Windows. Provided below are the steps in running Cleos using Linux/Ubuntu for Windows 10.

1. Get Ubuntu from Windows Store [Ubuntu Windows 10](https://www.microsoft.com/en-au/p/ubuntu/9nblggh4msv6?activetab=pivot:overviewtab "Ubuntu Windows 10")
1. Install Ubuntu from the Windows Store. The following error may appear:
   ```
   See https://aka.ms/wslinstall for details.
   Error: 0x8007007e
   Press any key to continue...
   ```
   If it does, ignore it and proceed to start PowerShell as Administrator and run the following command.
   ```
   Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Windows-Subsystem-Linux
   ```
1. Install Ubuntu and Login.
1. Within the Ubuntu shell, download and install the following EOS binaries
   ```
   wget https://github.com/EOSIO/eos/releases/download/v1.7.0/eosio_1.7.0-1-ubuntu-18.04_amd64.deb
   sudo apt install ./eosio_1.7.0-1-ubuntu-18.04_amd64.deb
   wget https://github.com/EOSIO/eosio.cdt/releases/download/v1.6.1/eosio.cdt_1.6.1-1_amd64.deb
   sudo apt install ./eosio.cdt_1.6.1-1_amd64.deb
   ```

### Deployment

#### EOS

This deployment utilizes EOS "jungle" testnet environment and needs to run within the Ubuntu shell.

1. Create EOS Wallet by running `cleos wallet create --to-console`. This will create a *default* wallet. Take note of the **WALLET_PASSWORD**
1. Generate a keypair for the master account `cleos create key --to-console`. Take note of the generated **PK_MASTER SK_MASTER** keypair.
1. Import the private keys into the newly created cleos wallet by running `cleos wallet import --private-key <SK_MASTER>`.
1. Navigate to the [EOS Jungle](https://monitor.jungletestnet.io/#account "Jungle") website for the Account Creation.
1. Create an EOS account by *Create Account*.  Use the generated **<PK_MASTER>** for both the *Owner Public Key* and *Active Public Key* fields. The *Account Name* you've entered is the **MASTER_ACCOUNT** designation.
1. The **MASTER_ACCOUNT** will need some RAM delegated for the smartcontract and transaction processing. Use the *faucet* on the [EOS Jungle](https://monitor.jungletestnet.io/#faucet "Jungle") site to give the Account Name **<MASTER_ACCOUNT>** some EOS. The account should now be funded with 100 EOS.
1. To stake EOS funds, run this command in cleos `cleos -u http://jungle2.cryptolions.io:80 system buyram <MASTER_ACCOUNT> <MASTER_ACCOUNT> "75 EOS"` (FYI: The -u parameter allows cleos to run this command in jungle testnet).

   <details>
   <summary>Expand this if your wallet got locked</summary>
   Should the wallet lock, run the following command

   ```
   cleos wallet unlock --password <WALLET_PASSWORD>
   ```

1. Generate a keypair for each of the 3 ORK nodes using cleos. 
   ```
   cleos create key --to-console
   cleos create key --to-console
   cleos create key --to-console
   ```
   Take a note of the 3 created keypairs **PK_ORK1 SK_ORK1**, **PK_ORK2 SK_ORK2**, **PK_ORK3 SK_ORK3**.
1. Using the Master Account, create a new EOS account for each of the 3 ORK nodes:
   ```
   cleos -u http://jungle2.cryptolions.io:80 system newaccount --stake-net "1.0000 EOS" --stake-cpu "1.0000 EOS" --buy-ram-kbytes 8 <MASTER_ACCOUNT> <ORK1xACCOUNT> <PK_ORK1> <PK_ORK1>
   cleos -u http://jungle2.cryptolions.io:80 system newaccount --stake-net "1.0000 EOS" --stake-cpu "1.0000 EOS" --buy-ram-kbytes 8 <MASTER_ACCOUNT> <ORK2xACCOUNT> <PK_ORK2> <PK_ORK2>
   cleos -u http://jungle2.cryptolions.io:80 system newaccount --stake-net "1.0000 EOS" --stake-cpu "1.0000 EOS" --buy-ram-kbytes 8 <MASTER_ACCOUNT> <ORK3xACCOUNT> <PK_ORK3> <PK_ORK3>
   ```
   *Note: **<ORKx_ACCOUNT>** needs to be 12 characters of a-z and 1-9 eg ork1accountx)*
1. Navigate to the onboarding folder `cd ../src/Raziel-Contracts/onboarding/`
1. Compile the onboarding contract: `eosio-cpp -abigen -o onboarding.wasm onboarding.cpp`
1. Upload the contract to EOS: `cleos -u http://jungle2.cryptolions.io:80 set contract <MASTER_ACCOUNT> ../onboarding -p <MASTER_ACCOUNT>@active`.

#### Credential Generation

The following steps occur outside the Ubuntu shell, under the location of the downloaded git repository.

1. Navigate to the Raziel.Generator folder and run `dotnet run 3`. This will generate 3 sets of credentials. One for each of the ORK nodes: 
11. **ORK1_CRYPTO_PK, ORK1_CRYPTO_SK, ORK1_CRYPTO_PASSWORD, ORK1_CRYPTO_KEY**
11. **ORK2_CRYPTO_PK, ORK2_CRYPTO_SK, ORK2_CRYPTO_PASSWORD, ORK2_CRYPTO_KEY**
11. **ORK3_CRYPTO_PK, ORK3_CRYPTO_SK, ORK3_CRYPTO_PASSWORD, ORK3_CRYPTO_KEY**

#### ORKs

1. Navigate to Raziel/Raziel.Ork. Open *appsettings.json* and populate it with the EOS Jungle settings:
   ```json
   {
       "Settings": {
         "BlockchainChainId": "e70aaab8997e1dfce58fbfac80cbbb8fecec7b99cf982a9444273cbc64c41473",
         "BlockchainEndpoint": "http://jungle2.cryptolions.io:80",
         "Onboarding": "tidecontract",
         "UsersTable": "tideusers",
         "FragmentsTable": "tidefrags"
        },
         "AllowedHosts": "*",
         "Logging": {
         "LogLevel": {
         "Default": "Information"
        }
      }
    }
    ```

1. Edit *appsettings.ork**n**.json* and populate using the following settings. Replace the variables with the details generated earlier. Perform this for all 3 ORK setting files (change **n** with the ORK number: 1, 2, 3).
   ```json
   {
     "Settings": {
       "Account": "<ORKnxACCOUNT>",
       "PublicKey": "<ORKn_CRYPTO_PK>",
       "PrivateKey": "<ORKn_CRYPTO_SK>",
       "EosPrivateKey": "<SK_ORKn>",
       "Password": "<ORKn_CRYPTO_PASSWORD>",
       "Key": "<ORKn_CRYPTO_KEY>"
     }
   }
   ```

1. Run the ORK nodes with the following commands in seperate terminals:

   ```
   dotnet run "https://localhost:5401" --environment "Ork1"
   dotnet run "https://localhost:5402" --environment "Ork2"
   dotnet run "https://localhost:5403" --environment "Ork3"
   ```

1. Test the nodes to ensure that it works by visiting https://localhost:5401/discover in a web browser. There should be a result similar to the following:
    ```json
    {
       "success": true,
       "content": {
           "account": "<ORK1xACCOUNT>",
           "url": "https://localhost:5401",
           "publicKey": "<ORKn_CRYPTO_PK>"
       },
       "error": null
    }
    ```

#### Database & Vendor

1. Navigate to Raziel/Raziel.Vendor. Edit *appsettings.json* and populate it with the following (feel free to use the pre-filled values):
   ```json
    {
      "VendorSettings": {
        "Password": "password",
        "Key": "dq8p8NiQ134WK94hUVlp%Ge%IiJXDsP9MPNFS%@qplGG#drX9!y7RWh&#e@xKC8ut%9ACw5^W^^6@SC"
      },
        "AllowedHosts": "*",
        "Logging": {
        "LogLevel": {
        "Default": "Information"
      }
      }
    }
   ```

1. Run `dotnet ef migrations add Initial` to create a migration. Run `dotnet ef database update` to create a sqlite database and push the required scaffolding.
1. Run the vendor using `dotnet run`. Take note of the endpoint shown on screen.

#### Account Creation

1. Navigate to Raziel/Raziel.Creator and run `npm install`.
1. Install webpack dependencies
   ```
   npm install --global webpack
   npm install --global webpack-cli
   ```
1. You can change *config.js* settings to reflect changes you've made to default values (If you didn't change those, don't worry about it)
1. Run the command `webpack` to compile the changes made to *config.js*.
1. Open *index.html* in a web browser, fill in the details. The added placeholder values are for brevity.
1. Click the *Create Account* button. When successful, 'Account created successfully' will appear on screen.

#### Frontend Setup

1. Navigate to \src\Raziel.Front run `npm install`.
1. Under *Raziel.Front\src\assets\js\config.js* you can set the ORK node endpoint array and the password if you changed those in vendor appsettings.json. If you didn't change, don't worry about it.
    ```
    {
      orkNodes: ["https://localhost:5401", "https://localhost:5402", "https://localhost:5403"],
      vendorEndpoint: "https://localhost:5001"
    }
    ```
1. Run the command `npm run build`. This will compile the website to the /dist folder.
1. Copy the contents of the \src\Raziel.Front\dist\ to your webserver folder (Xampp, Apache, IIS, etc).  If you don't have one installed, simply place the contents of that folder in the main drive root folder *C:\* (there should be 2 files and 2 folders).
1. Open *.\index.html* for the main website. 
1. Login using the account credentials created in section *Account Creation*.

### Get in touch!

[Tide Telegram group](https://t.me/TideFoundation)

[Tide Subreddit channel](https://www.reddit.com/r/TideFoundation)

  <a href="https://tide.org/licenses_tcosl-1-0-en">
    <img src="https://img.shields.io/badge/license-TCOS-green.svg" alt="license">
  </a>
</p>
