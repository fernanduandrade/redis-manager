<script lang="ts">
import { defineComponent, PropType, reactive } from 'vue'
import { RedisKey } from '../../domain'
import redis from '../../../api/redis'
import { useApplication } from '../../../common/store/index'


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
        let appStorage = useApplication()
        const localItems = reactive([...props.items])
        async function loadKey(key: RedisKey) {
            key.expanded = !key.expanded
            if(key.children!.length > 0)
                return
            if(key.type === 'keySpace') {
                const pattern = key.parent ? `${key.parent}:${key.name}` : key.name 
                const keysSpacesResponse = await redis.getKeysSpaces(props.connectionId,  pattern) as RedisKey[]
                const keyspacesResult = keysSpacesResponse.map(x => ({...x, children: []}))
                key.children = keyspacesResult
            } else {
                const pattern = key.parent ? `${key.parent}:${key.name}` : key.name 
                const keysSpacesResponse = await redis.getKeyValue(props.connectionId,  pattern)
                appStorage?.setCurrentCacheValue(keysSpacesResponse as string)
                appStorage?.setCurrentKey(pattern)
            }
            
        }
        return {
            loadKey, localItems, props
        }
    }
})

</script>

<template>
    <div class="ml-8 w-full " v-for="item in localItems" :key="item.id">
        <span class="key" v-if="item.type === 'keySpace'" @click="loadKey(item)">
            <i :class="[item.expanded ?  'pi pi-folder-open' : 'pi pi-folder']" />
            {{ item.name }} {{ item.type === 'keySpace' ? `(${item.count})` : '' }}
        </span>
        <span v-else class="key" @click="loadKey(item)">
            <i class="pi pi-key" />
            {{ item.name }}
        </span>
        <KeyFolder v-if="item.expanded && item.children!.length > 0" :connection-id="connectionId" :items="item.children!" />
    </div>
</template>

<style scoped>

.key {
    font-size: 20px;
    @apply hover:bg-slate-300 hover:cursor-pointer w-fit p-1 rounded-md block
}
</style>