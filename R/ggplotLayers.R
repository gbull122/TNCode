#Load some data
library("ggplot2")
data <- mpg
# Simple
ggplot()+layer(data=mpg,geom="point",mapping=aes(x=cty,y=hwy),stat="identity",position="identity",show.legend=FALSE)

# empty params
ggplot()+layer(data=mpg,geom="point",mapping=aes(x=cty,y=hwy),stat="identity",position_identity(),show.legend=FALSE,params = list())

#
p<-ggplot()+layer(data=mpg,geom="boxplot",mapping=aes(x=trans,y=hwy),stat="boxplot",position="dodge2",show.legend=FALSE,params = list(notch = FALSE, coef=1.5))

p<-ggplot()+layer(data=mpg,geom="boxplot",mapping=aes(x=trans,y=hwy),stat="boxplot",position_dodge2(),show.legend=FALSE,params = list(notch = FALSE, coef=1.5))

ggplot()+layer(data=mpg,geom="boxplot",mapping=aes(x=trans,y=hwy),stat="boxplot",position="dodge2",params=list(outlier.size=1.5,notchwidth=0.5,notch=FALSE,varwidth=FALSE,coef=1.5),show.legend=TRUE)

ggplot()+layer(data=mpg,geom="boxplot",mapping=aes(x=trans,y=hwy),stat="boxplot",position_dodge2(width = NULL, preserve = c("total","single"),padding=0.1,reverse=FALSE),params=list(outlier.size=1.5,notchwidth=0.5,notch=FALSE,varwidth=FALSE,coef=1.5),show.legend=TRUE)

ggplot()+geom_boxplot(data=mpg,aes(x=trans),outlier.size=1.5,notchwidth=0.5,notch=FALSE,coef=1.5,show.legend=TRUE)