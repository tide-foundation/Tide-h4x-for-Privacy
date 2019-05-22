# Tide h4x for Privacy Challenge

The concept for the challenge is quite simple: The details of 1 Bitcoin were stored in the simplest website setup: on a database record behind a web server. Anyone logging in to that website using the correct username and password will see those details. All typical defences around that set up were removed. No firewalls, no added security. It should be easy work for any hacker to crack that. The twist: Tide's unique protection mechanism was used on the data and the website authentication. Supposedly, even if one cracks the whole thing, it would be impractical to crack the authentication or extract the data. Anyone who does, the bitcoin is theirs, and Tide is back to the drawing board. This project is an open-source code repository for the challenge and provides ability to recreate the entire environment locally.

### Terminology

Below are concepts that are important to understand within the context of the Tide h4x challenge.

**Seeker** - Any organization or business seeking to engage with or collect consumer data.

**Vendor** - Any consumer-facing organization or business that collects, manages and stores consumer data.

**Consumer** - Any individual natural person that has a uniquely identified representation or data footprint (usually in the form of a user account or identity) in a Vendor’s database.

**Smart Contract** - a computer protocol intended to digitally facilitate, verify, or enforce the negotiation or performance of a contract.

**ORK** - Orchestrated Recluder of Keys - The Tide Protocol nodes

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

This guide assists you to replicate the entire environment using EOS jungle testnet (free), local deployment with 3 ORK nodes.
![alt text](https://github.com/tide-foundation/Tide-h4x-for-Privacy/blob/master/Tide%20h4x%20Local.png "Local Setup")

### Prerequisite

1. [.NET Core 2.2 Build apps - SDK](https://dotnet.microsoft.com/download/dotnet-core/2.2 ".net Core 2.2 Download")
1. [Node.js - LTS](https://nodejs.org/en/download/ "node.js Download")
1. [Cleos](https://developers.eos.io/eosio-nodeos/v1.2.0/docs/cleos-overview "Cleos")
1. Clone Repository `git clone https://github.com/tide-foundation/Tide-h4x-for-Privacy`

#### Installing Cleos in Windows 10

There are two options when running EOS in a Windows environment: Docker or Linux for Windows. Provided below are the steps in running Cleos using Linux/Ubuntu for Windows 10.

1. Get Ubuntu from Windows Store [Ubuntu Windows 10](https://www.microsoft.com/en-au/p/ubuntu/9nblggh4msv6?activetab=pivot:overviewtab "Ubuntu Windows 10")
1. Install Ubuntu from the Windows Store. Ignore the following error:
   ```
   See https://aka.ms/wslinstall for details.
   Error: 0x8007007e
   Press any key to continue...
   ```
   Open PowerShell as Administrator and run the following command.
   ```
   Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Windows-Subsystem-Linux
   ```
1. Install Ubuntu and Login.
1. Download and install EOS binaries
   ```
   wget https://github.com/EOSIO/eos/releases/download/v1.7.0/eosio_1.7.0-1-ubuntu-18.04_amd64.deb
   sudo apt install ./eosio_1.7.0-1-ubuntu-18.04_amd64.deb
   wget https://github.com/EOSIO/eosio.cdt/releases/download/v1.6.1/eosio.cdt_1.6.1-1_amd64.deb
   sudo apt install ./eosio.cdt_1.6.1-1_amd64.deb
   ```

### Deployment

#### EOS

This deployment utilizes EOS "jungle" testnet environment. 

1. Create EOS Wallet by running `cleos wallet create --to-console`. This will create a *default* wallet. Take note of the **WALLET_PASSWORD**
1. Generate a keypair for the master account `cleos create key --to-console`. Take note of the generated **PK_MASTER SK_MASTER** keypair.
1. Import the private keys into the newly created cleos wallet by running `cleos wallet import --private-key <SK_MASTER>`.
1. Navigate to the Jungle Testnet for the Account Creation [Jungle Test Net](https://monitor.jungletestnet.io/#account "Jungle").
1. Create a Jungle Testnet account by *Create Account*.  Use the generated PK_MASTER for the Owner Public Key and Active Public Key field. This will be the **MASTER_ACCOUNT**
1. The MASTER_ACCOUNT will need some RAM delegated for the smartcontract and transaction processing. Use the *faucet* on the [Jungle Test Net](https://monitor.jungletestnet.io/#faucet "Jungle") to give the **MASTER_ACCOUNT** some EOS. The Master Account should get 100 EOS.
1. To stake some EOS, run this command in cleos `cleos -u http://jungle2.cryptolions.io:80 system buyram <MASTER_ACCOUNT> <MASTER_ACCOUNT> "15 EOS"`. The -u parameter allows cleos to run this command in jungle testnet.

   <details>
   <summary>Unlock Wallet</summary>
   Should the wallet lock run the following command

   ```
   cleos wallet unlock --password <WALLET_PASSWORD>
   ```

1. Generate a keypair for each of the 3 ORK nodes using cleos. Take a note of the 3 created keys ORK 1-**PK_ORK1 SK_ORK1**, ORK 2-**PK_ORK2 SK_ORK2**, ORK 3-**PK_ORK3 SK_ORK3**.
1. Using the Master Account, create a new EOS account for each of the 3 ORK nodes by running
   ```
   cleos -u http://jungle2.cryptolions.io:80 system newaccount --stake-net "1.0000 EOS" --stake-cpu "1.0000 EOS" --buy-ram-kbytes 8 <MASTER_ACCOUNT> <ORK1xACCOUNT> <PK_ORK1> <PK_ORK1>
   cleos -u http://jungle2.cryptolions.io:80 system newaccount --stake-net "1.0000 EOS" --stake-cpu "1.0000 EOS" --buy-ram-kbytes 8 <MASTER_ACCOUNT> <ORK2xACCOUNT> <PK_ORK2> <PK_ORK2>
   cleos -u http://jungle2.cryptolions.io:80 system newaccount --stake-net "1.0000 EOS" --stake-cpu "1.0000 EOS" --buy-ram-kbytes 8 <MASTER_ACCOUNT> <ORK3xACCOUNT> <PK_ORK3> <PK_ORK3>
   ```
   *Note: ORKx_ACCOUNT needs to be 12 characters with a-z and 1-9 eg ork1accountx)*
1. Navigate to the onboarding folder `../src/Raziel-Contracts/onboarding/`
1. Compile the onboarding contract using `eosio-cpp -abigen -o onboarding.wasm onboarding.cpp`
1. Go up a folder (../src/Raziel-Contracts/) and run the command `cleos -u http://jungle2.cryptolions.io:80 set contract <MASTER_ACCOUNT> ./onboarding -p <MASTER_ACCOUNT>@active`.

#### Credential Generation

1. Navigate to the Raziel.Generator folder and run `dotnet run 4`. This will generate 4 sets of credentials. One for the master account and 1 for each ork node.

#### ORKs

1. Navigate to Raziel/Raziel.Ork. Open *appsettings.json* and populate it with the following code (change the variables should there be  a different blockchain):
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

1. Open *appsettings.ork1.json* and populate using the following code. Replace the variables with the details that was generated in `Credential Generation`. Perform this for all 3 ORK setting files.
   ```json
   {
     "Settings": {
       "Account": "The 12 character eos account you made for the ork nodes",
       "PublicKey": "The public key created in Raziel.Generator",
       "PrivateKey": "The private key created in Raziel.Generator",
       "EosPrivateKey": "The EOS private key associated to this ork node",
       "Password": "The password created in Raziel.Generator",
       "Key": "The key created in Raziel.Generator"
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
           "account": "yourorkaccount",
           "url": "https://localhost:5401",
           "publicKey": "ALdwxVNySq65hwkStfpSuuwz__EXAMPLE_PUBLIC_KEY__oD8PpStPQ0BXqHQd69NAD9LGzQLujEXg=="
       },
       "error": null
    }
    ```

#### Database & Vendor

1. Navigate to Raziel/Raziel.Vendor. Open *appsettings.json* and populate it with the following code (feel free to use the pre-filled values):
   ```json
    {
      "VendorSettings": {
        "Password": "Password",
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

1. Run `dotnet ef migrations add Initial` to create a migration. Run `dotnet ef database update` to push the scaffolding to your local database.
1. Run the vendor using `dotnet run`. Take note of the endpoint shown on screen.

#### Account Creation

1. Navigate to Raziel/Raziel.Creator and run `npm install`.
1. Install webpack dependencies
   ```
   npm install --global webpack
   npm install --global webpack-cli
   ```
1. Run the command `webpack` to compile the changes made to *config.js*.
1. Open *index.html* in a web browser, fill in the details. This added placeholder values is for brevity.
1. Open developer console by pressing F12, then click the *Create Account* button.
1. When successful, the 'Account created successfully' will appear in the console window.

#### Frontend Setup

1. Navigate to \src\Raziel.Front run `npm install`.
1. Open *Raziel.Front\src\assets\js\config.js*. Populate the ORK node endpoint array and enter the password you choose in vendor appsettings.json.
    ```
    {
      orkNodes: ["https://localhost:5401", "https://localhost:5402", "https://localhost:5403"],
      vendorEndpoint: "https://localhost:5001"
    }
    ```
1. Run the command `npm run build`. This will compile the website to the /dist folder.
1. Copy the contents of the \src\Raziel.Front\dist\ to your webserver folder.  Simply place the contents in the main drive root folder *C:\ (there should be 2 files and 2 folders)*.  An alternative is to have it in a webserver like Xampp, IIS or a htdocs folder. 
1. Open *.\index.html* for the main website. 
1. Login using the account credentials created in section *Account Creation*.

### Social

[Tide Telegram group](https://t.me/TideFoundation)

[Tide Subreddit channel](https://www.reddit.com/r/TideFoundation)

  <a href="https://tide.org/licenses_tcosl-1-0-en">
    <img src="https://img.shields.io/badge/license-TCOS-green.svg" alt="license">
  </a>
</p>
