#Load some data
library("ggplot2")
data <- mpg
# Simple
ggplot()+layer(data=mpg,geom="point",mapping=aes(x=cty,y=hwy),stat="identity",position="identity",show.legend=FALSE)

# empty params
ggplot()+layer(data=mpg,geom="point",mapping=aes(x=cty,y=hwy),stat="identity",position="identity",show.legend=FALSE,params = list())

#
ggplot()+layer(data=mpg,geom="boxplot",mapping=aes(x=trans,y=hwy),stat="boxplot",position="dodge2",show.legend=FALSE,params = list(notch = FALSE, coef=1.5))