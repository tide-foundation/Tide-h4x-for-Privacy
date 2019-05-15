<template>
  <div>
    <div class="n">{{ n }}</div>
    <div class="e" :class="{ edited: edited }"></div>
    <span class="c" v-html="c"> </span>
    <slot></slot>
  </div>
</template>

<script>
export default {
  props: ["t", "c", "n", "edited"]
}
</script>

<style scoped lang="scss">
div {
  height: 16px;
  display: flex;
  flex-direction: row;
  align-items: center;
}

.e {
  width: 5px !important;

  height: 20px;
  margin-left: 5px !important;
  margin-right: 15px;
}

@media only screen and (max-width: 335px) {
  .e {
    display: none;
  }
}

.c {
  color: #517535;
}

.edited {
  background-color: #eef18c;
}

.n {
  color: #2e7c9a;

  width: 30px;
  text-align: right;
  user-select: none;
  float: left;
  margin-left: 7px;
}
</style>
