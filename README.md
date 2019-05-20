# Tide h4x for Privacy Challenge

<p>
  <a href="https://tide.org/licenses_tcosl-1-0-en">
    <img src="https://img.shields.io/badge/license-TCOS-green.svg" alt="license">
  </a>
</p>
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

1. **Cryptide Library** - Tide encryption library
1. **h4x frontend Website** - website for this h4x challenge.
1. **h4x Tide SDK API** - API for the h4x challenge.
1. **h4x SQL Database** - Database for the h4x challenge.
1. **ORK Nodes** - Decentralized ORK nodes.

## Installation

This guide assists you to replicate the entire environment using EOS jungle testnet (free), local deployment with 3 ORK nodes.
![alt text](https://github.com/tide-foundation/Tide-h4x-for-Privacy/blob/master/Tide%20h4x%20Local.png "Local Setup")

### Prerequisite

1. [.NET Core 2.2 Build apps - SDK](https://dotnet.microsoft.com/download/dotnet-core/2.2 ".net Core 2.2 Download")
1. [Node.js - LTS](https://nodejs.org/en/download/ "node.js Download")
1. [Cleos](https://developers.eos.io/eosio-nodeos/v1.2.0/docs/cleos-overview "Cleos")
1. [SQL Express](https://www.microsoft.com/en-au/sql-server/sql-server-editions-express "SQL Express")
1. Clone Repository
   Using Windows Powershell

   ```
   wget "https://github.com/tide-foundation/Tide-h4x-for-Privacy/archive/master.zip" -outfile "h4x.zip"
   Expand-Archive "h4x.zip" -Force -DestinationPath "C:\code"
   ```

   <details>
   <summary>Linux</summary>
      
      ```
      git clone https://github.com/tide-foundation/Tide-h4x-for-Privacy
      ```
   </details>

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

1. Create EOS Wallet `cleos wallet create --to-console`. This will create a _default_ wallet. Take note of the **WALLET_PASSWORD**
1. Generate a keypair for your master account `cleos create key --to-console`. Take note of the generated **PK_MASTER SK_MASTER** keypair.
1. Import the private keys into your cleos wallet by running `cleos wallet import --private-key SK_MASTER`.
1. Navigate to Jungle Testnet for the Account Creation [Jungle Test Net](https://monitor.jungletestnet.io/#account "Jungle").
1. Create a Jungle Testnet account by _Create Account_. Use the generated PK_MASTER for the Owner Public Key and Active Public Key field. This will be your **MASTER_ACCOUNT**
1. The MASTER*ACCOUNT will need some RAM delegated to it for the smartcontract and transaction processing. Use the \_faucet* on the [Jungle Test Net](https://monitor.jungletestnet.io/#faucet "Jungle") to give your **MASTER_ACCOUNT** some EOS. Your main account should get 100 EOS.
1. In cleos, run the command `cleos -u http://jungle2.cryptolions.io:80 system buyram MASTER_ACCOUNT MASTER_ACCOUNT "15 EOS"`. The -u parameter is telling cleos to run this command using the jungle testnet.

   <details>
   <summary>Unlock Wallet</summary>
   Should the wallet lock run the following command

   ```
   cleos wallet unlock --password WALLET_PASSWORD
   ```

1. Generate a keypair for each of the 3 ORK nodes using cleos. Take a note of the 3 created keys ORK 1-**PK_ORK1 SK_ORK1**, ORK 2-**PK_ORK2 SK_ORK2**, ORK 3-**PK_ORK3 SK_ORK3**.
1. Using your Master Account create a new eos account for each of the 3 ORK nodes by running
   ```
   cleos -u http://jungle2.cryptolions.io:80 system newaccount --stake-net "1.0000 EOS" --stake-cpu "1.0000 EOS" --buy-ram-kbytes 8 MASTER_ACCOUNT ORK1xACCOUNT PK_ORK1 PK_ORK1
   cleos -u http://jungle2.cryptolions.io:80 system newaccount --stake-net "1.0000 EOS" --stake-cpu "1.0000 EOS" --buy-ram-kbytes 8 MASTER_ACCOUNT ORK2xACCOUNT PK_ORK2 PK_ORK2
   cleos -u http://jungle2.cryptolions.io:80 system newaccount --stake-net "1.0000 EOS" --stake-cpu "1.0000 EOS" --buy-ram-kbytes 8 MASTER_ACCOUNT ORK3xACCOUNT PK_ORK3 PK_ORK3
   ```
   _Note: ORKx_ACCOUNT needs to be 12 characters with a-z and 1-9 eg ork1accountx)_
1. Navigate to the onboarding folder `../src/Raziel-Contracts/onboarding/`
1. Compile the onboarding contract using `eosio-cpp -abigen -o onboarding.wasm onboarding.cpp`
1. Go up a folder (../src/Raziel-Contracts/) and run the command `cleos -u http://jungle2.cryptolions.io:80 set contract MASTER_ACCOUNT ./onboarding -p MASTER_ACCOUNT@active`.

#### Credential Generation

1. Navigate to the Raziel.Generator folder and run `dotnet run 4`. This will generate 4 sets of credentials. One for the master account and 1 for each ork node.

#### ORKs

1. Navigate to Raziel/Raziel.Ork and create 4 files with names:

```
appsettings.json
appsettings.Ork1.json
appsettings.Ork2.json
appsettings.Ork3.json
```

2. Open appsettings.json and populate it with the following code (change the variables if you're using a different blockchain):
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

3. Open appsettings.ork1.json and populate it with the following code. Replacing the variables with the details you generated in 'Credential Generation'. Do this for all 3 ork setting files.
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

4.  Run your ORK nodes with the following commands in seperate terminals:

```
dotnet run "https://localhost:5401" --environment "Ork1"
dotnet run "https://localhost:5402" --environment "Ork2"
dotnet run "https://localhost:5403" --environment "Ork3"
```

5.  Test the nodes are working by visiting https://localhost:5401/discover in a web browser. You should be get  a result similar to this one:
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

1. Navigate to Raziel/Raziel.Vendor and create a file called appsettings.json and populate it with the following code:
```json
{
  "VendorSettings": {
    "Password": "This can be any password you choose"
  }
}
```

2. Run `dotnet ef migrations add Initial` to create a migration. Run `dotnet ef database update` to push the scaffolding to your local database.
3. Run the vendor using `dotnet run`. Take note of the endpoint shown on screen.

#### Account Creation

1. Navigate to Raziel/Raziel.Creator and run `npm install`.
2. Open the config at /src/assets/js/config.js and edit the ork node array to reflect the 3 nodes you have running. Change the vendor endpoint to the above.
3. Edit the password variable with the password you choose to use in the vendor appsettings.
4. Run the command `webpack` to compile the changes made to config.js.
5. Open index.html in a web browser, fill in the details. We've added placeholder values for brevity.
6. Open developer console by pressing F12, then click the 'Create Account' button.
7. If it creates successfully, you should see 'Account created successfully' appear in the console.

#### Frontend Setup

1. Navigate to Raziel/Raziel.Front run `npm install`.
2. Open Raziel.Front\src\assets\js\config.js. Populate the ORK node endpoints array with the correct endpoints.
3. Open the .env.production file in the root folder and set your vendor endpoint there.
4. Run the command `npm run serve` to start up a development server and run the application.
5. You should not be able to login using the account credentials created in section 'Account Creation'

### Social

[Tide Telegram group](https://t.me/TideFoundation)

[Tide Subreddit channel](https://www.reddit.com/r/TideFoundation)
