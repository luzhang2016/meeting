<template>
    <div id="member-cell">
        <div class="avatar">
            <div :class="image">
                <span class="image-name">{{name}}</span>
            </div>
        </div>
        <div class="name">
            <span>{{member.name}}</span>
        </div>
    </div>
</template>
<script>
export default {
    props: {
        member: {}
    },
    computed: {
        name: function () {
            let str = this.member.name;
            return str.substr(str.length - 2);
        },
        image:function(){
            switch(this.member.flag){
                case 1:
                return 'image host';
                case 2:
                return 'image speaker';
                case 3:
                return 'image audience'
            }
        }
    }
}
</script>
<style lang="scss">
#member-cell {
    display: flex;
    background: #fff;
    padding: 10px 0;
    .avatar {
        height: 45px;
        margin: 5px 5px 5px 15px;
        .image {
            width: 45px;
            height: 45px;
            border-radius: 50%;
            flex: 0 0 40px;
            .image-name {
                color: white;
                font-size: 14px;
                display: inline-block;
                width: 45px;
                line-height: 45px;
                text-align: center;
            }
        }
        .host{
            background-color: #10DDDD;
        }
        .speaker{
            background-color: #D2A8F5;
        }
        .audience{
            background-color: #21BF07;
        }
    }
    .name {
        flex: 1;
        height: 45px;
        line-height: 45px;
        padding: 5px 5px 5px 10px;
        font-size: 16px;
    }
}

</style>
