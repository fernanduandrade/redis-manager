<script setup lang="ts">
import redis from './api/redis'
import { Connection, RedisKey } from './common/domain';
import { onMounted, ref } from 'vue'
import ConnectionForm from './common/components/ConnectionForm/index.vue'
import AddKeyForm from './common/components/AddKeyForm/index.vue'
import KeyFolder from './common/components/KeyFolder/index.vue'
import { memorySizeOf, storageGet, storageSet } from './common/logic'
import { useApplication } from './common/store/index'
import JsonViewer from './common/components/JsonViewer/index.vue'
import { storeToRefs } from 'pinia';

const appStorage = useApplication()

const { cacheValue, currentKey } = storeToRefs(appStorage)
const lastConnectionClicked = ref('')
const viewerOptions = ref([{ name: 'json' }, { name: 'text' }])
const selectedViewer = ref<{name: string}>({ name: 'text' })
const showConnectionFormModal = ref(false)
const showAddKeyFormModal = ref(false)
function closeConnectionForm(evt: boolean) {
  showConnectionFormModal.value = evt
}

function closeAddForm(evt: boolean) {
  showAddKeyFormModal.value = evt
}
const connections = ref<Array<Connection>>([])

const textValueExample = ref('')
function connectionName(connection: Connection) {
  if (connection.name)
    return connection.name

  return `${connection.host}@${connection.port}`
}

async function openConnection(connection: Connection) {
  lastConnectionClicked.value = connection.id
  connection.open = !connection.open
  await redis.openConnection(connection)

  if (connection.keyspaces!.length > 0)
    return


  const keysSpacesResponse = await redis.getKeysSpaces(connection.id, "") as RedisKey[]
  const keyspacesResult = keysSpacesResponse.map(x => ({ ...x, children: [] }))
  connection.keyspaces = keyspacesResult
}

function updateConnection(evt: Connection) {
  connections.value.push({ ...evt, keyspaces: [] })
  storageSet('userConnections', connections.value)
}


onMounted(() => {
  const connStorage = storageGet<Array<Connection>>('userConnections')
  connections.value = connStorage.map((x: Connection) => ({ ...x, open: false, keyspaces: [] }))
})


async function saveKeyValue() {
  await redis.updateKeyValue(lastConnectionClicked.value, currentKey.value, cacheValue.value)
}

</script>

<template>
  <ConnectionForm :visible="showConnectionFormModal" @close-form="closeConnectionForm"
    @update-connection="updateConnection" />
  <AddKeyForm :visible="showAddKeyFormModal" :connection-id="lastConnectionClicked" @close-form="closeAddForm" />
  <Splitter class="relative min-h-screen flex">
    <SplitterPanel :size="40" class="teste bg-[#FFFFFF] w-[500px] p-7 flex flex-col items-center gap-5 border-r-2 border-red-gray">
      <Button @click="showConnectionFormModal = true">Adicionar nova conexão</Button>
      <section class="w-full">
        <div class="card flex flex-col gap-2">
          <div class="bg-white flex flex-col p-4" v-for="(connection, index) in connections" :key="index">
            <div class="flex p-4 justify-between cursor-pointer hover:bg-slate-300 transition-all"
              @click="openConnection(connection)">
              <span class="font-semibold">{{ connectionName(connection) }}</span>
              <div class="flex gap-3 items-center">
                <div @click.stop="openConnection(connection)"
                  class="connection__action flex items-center justify-center rounded-md hover:bg-green-200 z-160cursor-pointer">
                  <i class="pi pi-refresh" />
                </div>
                <div @click.stop="showAddKeyFormModal  = true"
                  class="connection__action flex items-center justify-center rounded-md hover:bg-green-200 z-160cursor-pointer">
                  <i class="pi pi-plus" />
                </div>
                <div
                  class="connection__action flex items-center justify-center rounded-md hover:bg-green-200 z-10 cursor-pointer">
                  <i class="pi pi-home" />
                </div>
                <div
                  class="connection__action flex items-center justify-center rounded-md hover:bg-green-200 cursor-pointer">
                  <i class="pi pi-bars" />
                </div>
                <div :class="[connection.open ? 'rotate' : 'rotate-invert']"><i class="pi pi-chevron-down" /></div>
              </div>

            </div>

            <div v-if="connection.open" class="p-5 flex flex-col gap-4">
              <AutoComplete v-model="textValueExample" placeholder="Enter para pesquisar"
                :suggestions="connection.keyspaces" />
              <KeyFolder v-if="connection.keyspaces!.length > 0" :connection-id="connection.id"
                :items="connection.keyspaces!" />
            </div>
          </div>
        </div>
      </section>
    </SplitterPanel>

    <SplitterPanel :size="60" class="flex-1 flex text-2xl bg-[#F9FAFE]">
      <div v-show="currentKey || cacheValue" class="flex p-5 flex-col h-full w-full gap-6 bg-white shadow-md rounded-sm">
        <div class="flex gap-4 w-full items-center">
          <Select variant="filled" placeholder="Tipo de visualização" class="w-full md:w-80" v-model="selectedViewer"
            optionLabel="name" :options="viewerOptions" />
            <div>
              <span class="text-blue-600 font-semibold">Tamanho: {{ memorySizeOf(cacheValue) }}</span>
            </div>
        </div>
        <div class="justify-center items-center">
          <JsonViewer v-if="selectedViewer.name === 'json'" :content="cacheValue!" />
          <div class="flex flex-col gap-6" v-else>
            <textarea v-model="cacheValue" class="h-[200px]" />
            <Button class="w-[200px] self-end" label="Salvar" @click="saveKeyValue" />
          </div>
          
        </div>

      </div>
    </SplitterPanel>
  </Splitter>

</template>

<style scoped>
.connection__action {
  width: 30px;
  height: 30px;
}
</style>