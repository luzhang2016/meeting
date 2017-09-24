const Koa = require('koa')
const app = new Koa()
const views = require('koa-views')
const json = require('koa-json')
const onerror = require('koa-onerror')
const bodyParser = require('koa-bodyparser')
const logger = require('koa-logger')

const index = require('./webservice/routes/index')
const users = require('./webservice/routes/users')

// error handler
onerror(app)

// middlewares
app.use(bodyParser())
app.use(json())
app.use(logger())

app.use(require('koa-static')(__dirname + '/dist'))

// logger
app.use(async(ctx, next) => {
    const start = new Date()
    await next()
    const ms = new Date() - start;
    ctx.type = ('text/event-stream');
    console.log(`${ctx.method} ${ctx.url} - ${ms}ms`)
})

// routes
app.use(index.routes(), index.allowedMethods())
app.use(users.routes(), users.allowedMethods())

app.listen(3001);
console.log('app started at port 3001...');