<template>
  <div id="console-container">
    <section id="topbar">
      <section id="menu">
        <div
          :class="{ active: currentTab == 'problems' }"
          @click="changeTab('problems')"
        >
          PROBLEMS
        </div>
        <div
          :class="{ active: currentTab == 'about' }"
          @click="changeTab('about')"
        >
          ABOUT
        </div>
        <div
          :class="{ active: currentTab == 'proof' }"
          @click="changeTab('proof')"
        >
          PROOF
        </div>
        <div
          :class="{ active: currentTab == 'terminal' }"
          @click="changeTab('terminal')"
        >
          TERMINAL
        </div>
      </section>
      <section id="controls">
        <button @click="toggleExpand">{{ expanded ? "▼" : "▲" }}</button>
      </section>
    </section>
    <section id="console">
      <section v-if="currentTab == 'problems'">
        <p>I got 99 problems but a breach ain't one</p>
      </section>
      <section v-if="currentTab == 'about'">
        <p>Welcome to Tide's decentralized username / password authentication live proof of concept.</p>
        <p>The first step in individuals claiming ownership of their online identity, by removing</p>
        <p>reliance on any third party for authentication. It can also help remove the</p>
        <p>unnecessary liability organizations carry holding credentials and performing</p>
        <p>authentication themselves.</p>
        <p>Use dummy credentials: Admin / Password to test</p>
        <br />
        <a
          class="success"
          target="_blank"
          href="https://Tide.org/splintering"
          >Splintering Paper
        </a>
        <a
          class="success"
          target="_blank"
          href="https://Tide.org/Whitepaper"
          >Whitepaper
        </a>
                <a
          class="success"
          target="_blank"
          href="https://github.com/tide-foundation"
          >Github
        </a>
      </section>
      <section v-if="currentTab == 'proof'">
        <p class="c">
          // Proof of Bounty
        </p>
        <br />
        <p>
          Bitcoin wallet ID:
          <span>
            <a
              class="success"
              target="_blank"
              href="https://www.blockchain.com/btc/address/1EBwDuG8EvG9wDrgVconUZ3kGQg95H94ne"
              >1EBwDuG8EvG9wDrgVconUZ3kGQg95H94ne</a
            ></span
          >
        </p>

        <p>
          Signature:
          <span
            ><a
              class="success break"
              target="_blank"
              href="https://brainwalletx.github.io/#verify?vrAddr=1EBwDuG8EvG9wDrgVconUZ3kGQg95H94ne&vrMsg=I%20got%2099%20problems%20but%20a%20breach%20ain%27t%20one%20-%20h4x.tide.org&vrSig=H96xUWdfN7mgpwANJNvCecxAQvGBP7Ly2pG1BC2aEId7f2CgUSZaYXjLEs%2BHtJUF1%2BnrwNUJbTSoDJkzetXeGjo%3D"
              >H96xUWdfN7mgpwANJNvCecxAQvGBP7Ly2pG1BC2aEId7f2CgUSZaYXjLEs+HtJUF1+nrwNUJbTSoDJkzetXeGjo=</a
            ></span
          >
        </p>
        <p>Exciting news! Our Bitcoin bounty has finally been claimed. </p>
        <p>6mnth & millions of hacks ago, we invited hackers to hammer </p>
        <p>our 1st public POC. Community engagement was brilliant. </p>
        <p>We’ve learned and improved a lot. Full report, new version </p>
        and more bounties coming soon!</p>
      </section>
      <section v-if="currentTab == 'terminal'">
        <p
          v-for="(item, index) in items"
          :key="index"
          :class="{
            error: item.type == 'error',
            success: item.type == 'success'
          }"
          v-html="item.msg"
        ></p>
      </section>
    </section>
  </div>
</template>

<script>
export default {
  data() {
    return {
      currentTab: 'terminal',
      items: [],
      expanded: false
    }
  },
  created() {
    this.$bus.$on('trigger-expand', () => this.toggleExpand());
    this.$bus.$on('logging in', () => this.currentTab = 'terminal');
    document.addEventListener("log", (e) => {
      var index = this.items.findIndex(i => i.id == e.detail.id);
      if (index != -1) this.$set(this.items, index, e.detail)
      else this.items.push(e.detail)

      setTimeout(() => {
        var objDiv = document.getElementById("console");
        objDiv.scrollTop = objDiv.scrollHeight;
      }, 50)
    });
  },
  methods: {
    toggleExpand() {
      this.expanded = !this.expanded;
      this.$bus.$emit('toggle', this.expanded);
    },
    changeTab(tab) {
      if (!this.expanded) this.toggleExpand();
      this.currentTab = tab;
    }
  }
}
</script>

<style lang="scss" scoped>
#console-container {
  width: 100%;
  height: 100%;
  background-color: #282a36;
  border-top: 1px solid #9073be;
  display: flex;
  flex-direction: column;
  font-size: 12px;
}

#topbar {
  display: flex;
  justify-content: space-between;
  height: 30px;
  padding: 0 20px;

  section {
    display: flex;

    div {
      margin-right: 20px;
      color: #54628a;
      display: flex;
      line-height: 30px;
    }

    div.active {
      color: white;
      border-bottom: #ff79c6 1px solid;
    }
  }
}

#menu {
  div {
    cursor: pointer;
  }

  div:hover {
    color: white;
  }
}

#console {
  flex: 1;
  margin: 10px 0;
  padding: 0 20px;
  color: white;
  p {
    margin: 0px;
    padding: 0px;
    line-height: 17px;
  }

  p.error {
    color: red;
  }

  overflow: auto;
}

#controls {
  height: 30px;
  margin-top: 7px;
  button {
    min-width: 10px !important;
    width: 20px !important;
    padding: 0px;

    background: #9073be;
  }

  button:hover {
    background: #b08fe4;
  }
}

.break {
  word-break: break-word;
}
</style>
