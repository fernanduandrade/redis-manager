<script lang="ts">
import { defineComponent, PropType, reactive } from 'vue';
import { RedisKey } from '../../domain';
import redis from '../../../api/redis';
export default defineComponent({
    name: 'KeyFolder',
    props: {
        items: {
            type: Object as PropType<Array<RedisKey>>,
            required: true
        },
        connectionId: {
            type: String,
            required: true
        }
    },
    setup(props) {
        const localItems = reactive([...props.items])
        async function loadKey(key: RedisKey) {
            if(key.type === 'keySpace') {
                key.expanded = !key.expanded
            }
            const pattern = key.parent ? `${key.parent}:${key.name}` : key.name 
            const keysSpacesResponse = await redis.getKeysSpaces(props.connectionId,  pattern) as RedisKey[]
            const keyspacesResult = keysSpacesResponse.map(x => ({...x, children: []}))
            key.children = keyspacesResult
        }
        return {
            loadKey, localItems, props
        }
    }
})

</script>

<template>
    <div class="m-0 w-full" v-for="item in localItems" :key="item.id">
        <span class="key" v-if="item.type === 'keySpace'" @click="loadKey(item)">
            <i :class="[item.expanded ?  'pi pi-folder-open' : 'pi pi-folder']" />
            {{ item.name }} {{ item.type === 'keySpace' ? `(${item.count})` : '' }}
        </span>
        <span v-else class="key" @click="loadKey(item)">
            <i class="pi pi-key" />
            {{ item.name }}
        </span>
        <KeyFolder class="ml-4" v-if="item.expanded && item.children!.length > 0" :connection-id="connectionId" :items="item.children!" />
    </div>
</template>

<style scoped>

.key {
    @apply hover:bg-slate-300 hover:cursor-pointer
}
</style>