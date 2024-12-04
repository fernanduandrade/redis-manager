<script setup lang="ts">
import redis from './api/redis'
import { Connection, RedisKey } from './common/domain';
import { onMounted, ref, toRaw } from 'vue'
import ConnectionForm from './common/components/ConnectionForm/index.vue'
import KeyFolder from './common/components/KeyFolder/index.vue'
import { storageGet, storageSet } from './common/logic';

const showConnectionFormModal = ref(false)
function closeFormEvent(evt: boolean) {
  showConnectionFormModal.value = evt
}
const connections = ref<Array<Connection>>([])

const keyspaces = ref<Array<RedisKey>>([])

const textValueExample = ref('')
function connectionName(connection: Connection) {
  if (connection.name)
    return connection.name

  return `${connection.host}@${connection.port}`
}

async function openConnection(connection: Connection) {
  connection.open = !connection.open
  await redis.openConnection(connection)
  
  const keysSpacesResponse = await redis.getKeysSpaces(connection.id, "") as RedisKey[]
  const keyspacesResult = keysSpacesResponse.map(x => ({...x, children: []}))
  keyspaces.value = keyspacesResult 
}

function updateConnection(evt: Connection) {
  connections.value.push(toRaw(evt))
  storageSet('userConnections', connections.value)
}

onMounted(() => {
  connections.value = storageGet<Array<Connection>>('userConnections')
})

</script>

<template>
  <ConnectionForm :visible="showConnectionFormModal" @close-form="closeFormEvent" @update-connection="updateConnection" />
  <main class="relative min-h-screen flex">
    <div class="bg-[#FFFFFF] w-[500px] p-7 flex flex-col items-center gap-5 border-r-2 border-red-gray">
      <Button @click="showConnectionFormModal = true">Adicionar nova conexão</Button>
      <section>
        <div class="card flex flex-col gap-2">
          <div class="bg-white flex flex-col p-4" v-for="(connection, index) in connections" :key="index">
            <div class="flex p-4 justify-between cursor-pointer hover:bg-slate-300 transition-all" @click="openConnection(connection)">
              <span class="font-semibold">{{ connectionName(connection) }}</span>
              <div class="flex gap-3 items-center">
                <div class="connection__action flex items-center justify-center rounded-md hover:bg-green-200 z-10 cursor-pointer"><i class="pi pi-home" /></div>
                <div class="connection__action flex items-center justify-center rounded-md hover:bg-green-200 cursor-pointer"><i class="pi pi-bars" /></div>
                <div :class="[connection.open ? 'rotate' : '']" ><i class="pi pi-chevron-down" /></div>
              </div>

            </div>

            <div v-if="connection.open" class="p-5 flex flex-col gap-4">
              <AutoComplete v-model="textValueExample" placeholder="Enter para pesquisar" :suggestions="keyspaces" />
              <KeyFolder v-if="keyspaces.length > 0" :connection-id="connection.id" :items="keyspaces" />
            </div>
          </div>
        </div>
      </section>
    </div>

    <div class="flex-1 flex justify-center items-center p-4 text-2xl bg-[#F9FAFE]">
      <p class="text-[100px] font-semibold">se liga aqui mané</p>
      <br />
      <p class="text-[100px] font-semibold">{{ textValueExample }}</p>
    </div>
  </main>

</template>

<style scoped>
.connection__action {
  width: 30px;
  height: 30px;
}

.rotate {
  animation: rotation .4s forwards;
}

@keyframes rotation {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(180deg);
  }
}
</style>