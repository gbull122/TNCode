#Load some data
library("ggplot2")
data <- mpg
#Normal
qqnorm(data$hwy, pch = 16, col = "Blue", sub = "Variable")
box(which = "figure", lty = 1, col = "black")
grid(lty = 2)
qqline(data$hwy, col = "red", lwd = 2)
#Variance
boxplot(list(data$hwy, data$cty), names = c("A", "B"))
box(which = "figure", lty = 1, col = "black")
grid(lty = 2, nx = NA, ny = NULL, col = "lightgray")
#T-Test
aHist <- hist(data$hwy)
aDensity <- density(data$hwy)
multi <- aHist$counts / aHist$density
aDensity$y <- aDensity$y * multi[1]
if (max(aDensity$y) > max(aHist$counts)) {
  plot(aHist, ylim = c(0, max(aDensity$y)))
} else {
  plot(aHist, ylim = c(0, max(aHist$counts)))
}
box(which = "figure", lty = 1, col = "black")
abline(v = mean(data$hwy),col="red",lwd=2)
lines(aDensity, col = "darkblue", lwd = 2)
#2 way Test
temp <- t.test(data$hwy, data$cty, paired = FALSE, alternative = "two.sided", conf.level = 0.95)
myResult <- capture.output(print(temp))
d1 <- list(density(data$hwy), density(data$cty))
plot(NA, xlim = range(sapply(d1, "[", "x")), ylim = range(sapply(d1, "[", "y")))
box(which = "figure", lty = 1, col = "black")
grid(lty = 2, col = "lightgray")
mapply(lines, d1, lwd=2,col = 1:length(result))
legend("topright", legend = c("A", "B"), fill = 1:2)
#ANOVA
aModel<-aov(data$cty~as.factor(data$cyl))