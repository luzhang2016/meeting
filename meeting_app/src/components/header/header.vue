<template>
    <div class="header is-fixed">
        <div class="header-button">
            <a onclick="window.history.go(-1)">
                <i class="fa fa-angle-left fa-2x icon" aria-hidden="true">
                    <slot name="left" class="left"></slot>
                </i>
            </a>
        </div>
        <span class="header-title">{{title}}</span>
        <slot name="right" class="right"></slot>
    </div>
</template>

<script>
export default {
    props: {
        title: {
            type: String,
            default: ''
        },
    },
}
</script>

<style lang="scss" scoped>
.header {
    height: 45px;
    background-color: #26A2FF;
    color: white;
    text-align: center;
    font-size: 18px;
    .header-button {
        position: absolute;
        left: 10px;
        top: 5px;
        font-size: 16px;
        .icon {
            vertical-align: middle;
            color:white;
            .left {
                position: absolute;
                top: 9px;
                left: 18px;
                min-width: 33px;
                font-size: 16px;
            }
        }
    }
    .header-title {
        line-height: 45px;
    }
    .right {
        position: absolute;
        right: 11px;
        top: 10px;
        font-size: 16px;
    }
}

.is-fixed {
    top: 0;
    right: 0;
    left: 0;
    position: fixed;
    z-index: 50;
}
</style>